using FastArena.Core.Domain.Items;
using FastArena.Core.Exceptions;
using FastArena.Core.Interfaces.App;
using FastArena.Core.Interfaces.Storages;
using FastArena.Core.Models;

namespace FastArena.Core.Services;

public class ShopService : IShopService
{
    private readonly IItemStorage _itemStorage;
    private readonly IItemService _itemService;
    private readonly IUserStorage _userStorage;
    private readonly IActivityStateService _activityStateService;
    private readonly IHeroStorage _heroStorage;

    public ShopService(
        IItemStorage itemStorage,
        IItemService itemService,
        IUserStorage userStorage,
        IActivityStateService activityStateService,
        IHeroStorage heroStorage)
    {
        _itemStorage = itemStorage;
        _itemService = itemService;
        _userStorage = userStorage;
        _activityStateService = activityStateService;
        _heroStorage = heroStorage;
    }

    public async Task<ShopCatalog> GetAvailableItemsAsync(Guid userId)
    {
        await EnsureShopAccessibleAsync(userId);

        // Get available potions
        var potions = await _itemStorage.GetByTypeAsync(ItemType.POTION);
        var moneyItem = await _itemService.GetBaseMoneyItemAsync();

        // Map to ShopItemDto with calculated sell prices
        var shopItems = potions.Select(item => new ShopItem
        {
            ItemId = item.Id,
            Name = item.Name,
            Description = item.Description,
            ItemImage = item.ItemImage,
            SellPrice = CalculateSellPrice(item.BaseCost),
            CanBeFolded = item.CanBeFolded,
        }).ToList();

        return new ShopCatalog
        {
            MoneyItem = moneyItem,
            Items = shopItems,
        };
    }

    public async Task<List<ShopHeroItem>> GetHeroItemsAsync(Guid userId)
    {
        var hero = await EnsureShopAccessibleAsync(userId);
        var equippedCellIds = hero.EquippedSlots?
            .Where(s => s.HeroItemCellId.HasValue)
            .Select(s => s.HeroItemCellId!.Value)
            .ToHashSet()
            ?? new HashSet<Guid>();

        return hero.Items?
            .Where(i => i.Item != null
                && i.Item.Type != ItemType.MONEY
                && !equippedCellIds.Contains(i.Id))
            .Select(i => new ShopHeroItem
            {
                HeroItemCellId = i.Id,
                ItemId = i.ItemId,
                Name = i.Item!.Name,
                Description = i.Item.Description,
                ItemImage = i.Item.ItemImage,
                Amount = i.Amount,
                CanBeFolded = i.Item.CanBeFolded,
                BuyPrice = CalculateBuyPrice(i.Item.BaseCost),
            })
            .ToList()
            ?? new List<ShopHeroItem>();
    }

    public async Task ExecuteTransactionAsync(Guid userId, ShopTransactionModel model)
    {
        var hero = await EnsureShopAccessibleAsync(userId);

        var sellItems = model.SellItems
            .Where(i => i.Quantity > 0)
            .GroupBy(i => i.HeroItemCellId)
            .Select(g => new HeroItemTakeRequest
            {
                HeroItemCellId = g.Key,
                Quantity = g.Sum(x => x.Quantity),
            })
            .ToList();

        var buyItems = model.BuyItems
            .Where(i => i.Quantity > 0)
            .GroupBy(i => i.ItemId)
            .Select(g => new ShopBuyRequestItem
            {
                ItemId = g.Key,
                Quantity = g.Sum(x => x.Quantity),
            })
            .ToList();

        var heroItemsByCellId = hero.Items?
            .Where(i => i.Item != null)
            .ToDictionary(i => i.Id)
            ?? new Dictionary<Guid, Domain.Heroes.HeroItemCell>();
        var equippedCellIds = hero.EquippedSlots?
            .Where(s => s.HeroItemCellId.HasValue)
            .Select(s => s.HeroItemCellId!.Value)
            .ToHashSet()
            ?? new HashSet<Guid>();

        foreach (var sellItem in sellItems)
        {
            if (!heroItemsByCellId.TryGetValue(sellItem.HeroItemCellId, out var heroItemCell))
            {
                throw new ActionDeniedException("Hero does not own one of the selected items.");
            }

            if (heroItemCell.Item?.Type == ItemType.MONEY)
            {
                throw new ActionDeniedException("Money balance is calculated automatically and must not be sent as a sold item.");
            }

            if (heroItemCell.Amount < sellItem.Quantity)
            {
                throw new ActionDeniedException("Hero does not have enough quantity of one of the selected items.");
            }

            if (equippedCellIds.Contains(sellItem.HeroItemCellId))
            {
                throw new ActionDeniedException("Equipped items cannot be sold.");
            }
        }

        var buyItemIds = buyItems.Select(i => i.ItemId).ToList();
        var buyCatalogItems = await _itemStorage.GetByIdsAsync(buyItemIds);
        var buyCatalogById = buyCatalogItems.ToDictionary(i => i.Id);

        foreach (var buyItem in buyItems)
        {
            if (!buyCatalogById.TryGetValue(buyItem.ItemId, out var item))
            {
                throw new ActionDeniedException("One of the selected shop items does not exist.");
            }

            if (item.Type != ItemType.POTION)
            {
                throw new ActionDeniedException("One of the selected shop items is not available in the shop.");
            }
        }

        var sellTotal = sellItems.Sum(sellItem =>
        {
            var heroItemCell = heroItemsByCellId[sellItem.HeroItemCellId];
            return CalculateBuyPrice(heroItemCell.Item!.BaseCost) * sellItem.Quantity;
        });

        var buyTotal = buyItems.Sum(buyItem =>
        {
            var item = buyCatalogById[buyItem.ItemId];
            return CalculateSellPrice(item.BaseCost) * buyItem.Quantity;
        });

        var heroMoneyAmount = hero.Items?
            .Where(i => i.Item?.Type == ItemType.MONEY)
            .Sum(i => i.Amount)
            ?? 0;

        var heroMoneySpent = Math.Max(0, buyTotal - sellTotal);
        var shopMoneyAdded = Math.Max(0, sellTotal - buyTotal);

        if (heroMoneyAmount < heroMoneySpent)
        {
            throw new ActionDeniedException("Hero does not have enough money to balance the transaction.");
        }

        var boughtItems = buyItems.Select(buyItem => new Domain.Heroes.GivenItem
        {
            Item = buyCatalogById[buyItem.ItemId],
            Amount = buyItem.Quantity,
        }).ToList();

        await _heroStorage.ExchangeHeroItemsAsync(
            hero.Id,
            sellItems,
            boughtItems,
            heroMoneySpent,
            shopMoneyAdded);
    }

    /// <summary>
    /// Calculate sell price with +50% markup from base cost.
    /// Uses standard rounding: 0.5 rounds up to 1.
    /// </summary>
    private int CalculateSellPrice(int baseCost)
    {
        return (int)Math.Round(baseCost * 1.5);
    }

    private int CalculateBuyPrice(int baseCost)
    {
        return (int)Math.Round(baseCost * 0.5);
    }

    private async Task<Domain.Heroes.Hero> EnsureShopAccessibleAsync(Guid userId)
    {
        var user = await _userStorage.GetAsync(userId);
        if (!user.SelectedHeroId.HasValue)
        {
            throw new EntityNotFoundException("No hero is selected");
        }

        var isBusy = await _activityStateService.IsBusyAsync(userId);
        if (isBusy)
        {
            throw new ActionDeniedException("Hero is on adventure and cannot access the shop");
        }

        return await _heroStorage.GetAsync(user.SelectedHeroId.Value);
    }
}
