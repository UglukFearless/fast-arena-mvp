using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Items;
using FastArena.Core.Interfaces.Storages;
using FastArena.Core.Models;
using FastArena.Dal.Entities;
using FastArena.Dal.Profiles;
using Microsoft.EntityFrameworkCore;

namespace FastArena.Dal.Storages;

public class HeroStorage : IHeroStorage
{
    private ApplicationContext _context;
    public HeroStorage(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Hero> CreateAsync(HeroCreationModel model)
    {
        var newHero = HeroProfile.Map(model);
        _context.Heroes.Add(newHero);
        await _context.SaveChangesAsync();

        return HeroProfile.Map(newHero);
    }

    public async Task<ICollection<Hero>> GetAllByUserIdAsync(Guid userId)
    {
        var heroes = await _context.Heroes
            .Where(h => h.UserId == userId)
            .Include(h => h.Portrait)
            .AsNoTracking().ToListAsync();
        return HeroProfile.Map(heroes, true);
    }

    public async Task<ICollection<Hero>> GetAllWithIncludesByUserIdAsync(Guid userId)
    {
        var heroes = await _context.Heroes
            .Where(h => h.UserId == userId)
            .Include(h => h.Portrait)
            .Include(h => h.Items)
            .ThenInclude(ic => ic.Item)
            .ThenInclude(i => i!.Effects)
            .Include(h => h.Items)
            .ThenInclude(ic => ic.Item)
            .ThenInclude(i => i!.AllowedSlots)
            .Include(h => h.EquippedSlots)
            .ThenInclude(es => es.HeroItemCell)
            .ThenInclude(ic => ic!.Item)
            .ThenInclude(i => i!.Effects)
            .Include(h => h.Results)
            .ThenInclude(r => r.Portrait)
            .AsNoTracking()
            .ToListAsync();
        return HeroProfile.Map(heroes, true);
    }

    public async Task<ICollection<Hero>> GetAllWithIncludesAsync()
    {
        var heroes = await _context.Heroes
            .Include(h => h.Portrait)
            .Include(h => h.Items)
            .ThenInclude(ic => ic.Item)
            .ThenInclude(i => i!.Effects)
            .Include(h => h.Items)
            .ThenInclude(ic => ic.Item)
            .ThenInclude(i => i!.AllowedSlots)
            .Include(h => h.EquippedSlots)
            .ThenInclude(es => es.HeroItemCell)
            .ThenInclude(ic => ic!.Item)
            .ThenInclude(i => i!.Effects)
            .Include(h => h.Results)
            .ThenInclude(r => r.Portrait)
            .AsNoTracking()
            .ToListAsync();
        return HeroProfile.Map(heroes, true);
    }

    public async Task<Hero> GetAsync(Guid id)
    {
        var hero = await _context.Heroes
            .Include(h => h.Portrait)
            .Include(h => h.Items)
            .ThenInclude(ic => ic.Item)
            .ThenInclude(i => i!.Effects)
            .Include(h => h.Items)
            .ThenInclude(ic => ic.Item)
            .ThenInclude(i => i!.AllowedSlots)
            .Include(h => h.EquippedSlots)
            .ThenInclude(es => es.HeroItemCell)
            .ThenInclude(ic => ic!.Item)
            .ThenInclude(i => i!.Effects)
            .Include(h => h.Results)
            .ThenInclude(r => r.Portrait)
            .AsNoTracking()
            .FirstAsync(h => h.Id == id);
        return HeroProfile.Map(hero, true);
    }

    public async Task<List<HeroItemCell>> GiveItemsToHeroAsync(Guid heroId, ICollection<GivenItem> items)
    {
        var heroDal = await GetWithItemsAsync(heroId);

        AddItemsToHero(heroDal, items);

        await _context.SaveChangesAsync();

        var newItems = await _context.HeroItemCells.Where(ic => ic.HeroId == heroId).Include(ic => ic.Item).ToListAsync();

        return HeroItemCellProfiles.Map(newItems, true);
    }

    public async Task ExchangeHeroItemsAsync(
        Guid heroId,
        ICollection<HeroItemTakeRequest> itemsToTake,
        ICollection<GivenItem> itemsToGive,
        int moneyToTake,
        int moneyToGive)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var heroDal = await GetWithItemsAsync(heroId);

        foreach (var itemToTake in itemsToTake)
        {
            var heroItemCell = heroDal.Items.FirstOrDefault(i => i.Id == itemToTake.HeroItemCellId);
            if (heroItemCell == null)
            {
                throw new InvalidOperationException("Hero item is missing during transaction application.");
            }

            heroItemCell.Amount -= itemToTake.Quantity;
            if (heroItemCell.Amount <= 0)
            {
                heroDal.Items.Remove(heroItemCell);
                _context.HeroItemCells.Remove(heroItemCell);
            }
        }

        var moneyCell = heroDal.Items.FirstOrDefault(i => i.Item != null && i.Item.Type == ItemType.MONEY);
        if (moneyToTake > 0)
        {
            if (moneyCell == null || moneyCell.Amount < moneyToTake)
            {
                throw new InvalidOperationException("Hero does not have enough money during transaction application.");
            }

            moneyCell.Amount -= moneyToTake;
            if (moneyCell.Amount <= 0)
            {
                heroDal.Items.Remove(moneyCell);
                _context.HeroItemCells.Remove(moneyCell);
                moneyCell = null;
            }
        }

        if (moneyToGive > 0)
        {
            if (moneyCell != null)
            {
                moneyCell.Amount += moneyToGive;
            }
            else
            {
                var moneyItem = await _context.Items.FirstAsync(i => i.Type == ItemType.MONEY && i.BaseCost == 1);
                _context.HeroItemCells.Add(new HeroItemCellDal
                {
                    Id = Guid.NewGuid(),
                    HeroId = heroId,
                    ItemId = moneyItem.Id,
                    Amount = moneyToGive,
                });
            }
        }

        AddItemsToHero(heroDal, itemsToGive);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
    }

    public async Task<Hero> UpdateHeroAsync(Hero hero)
    {
        var storedHero = await _context.Heroes.FirstAsync(h => h.Id == hero.Id);

        storedHero.Name = hero.Name;
        storedHero.Sex = hero.Sex;
        storedHero.Level = hero.Level;
        storedHero.Experience = hero.Experience;
        storedHero.IsAlive = hero.IsAlive;
        storedHero.MaxHealth = hero.MaxHealth;

        await _context.SaveChangesAsync();

        return await GetAsync(hero.Id);
    }

    #region private methods
    private void AddItemsToHero(HeroDal heroDal, ICollection<GivenItem> items)
    {
        var foldedItems = items.Where(gi => gi.Item.CanBeFolded);
        var notFoldedItems = items.Where(gi => !gi.Item.CanBeFolded);

        foreach (var foldedItem in foldedItems)
        {
            var existItem = heroDal.Items.FirstOrDefault(i => i.ItemId == foldedItem.Item.Id);
            if (existItem != null)
            {
                existItem.Amount += foldedItem.Amount;
            }
            else
            {
                var newItemCell = new HeroItemCellDal
                {
                    Id = Guid.NewGuid(),
                    HeroId = heroDal.Id,
                    ItemId = foldedItem.Item.Id,
                    Amount = foldedItem.Amount,
                };
                heroDal.Items.Add(newItemCell);
                _context.HeroItemCells.Add(newItemCell);
            }
        }

        foreach (var notFoldedItem in notFoldedItems)
        {
            for (int i = 0; i < notFoldedItem.Amount; i++)
            {
                var newItemCell = new HeroItemCellDal
                {
                    Id = Guid.NewGuid(),
                    HeroId = heroDal.Id,
                    ItemId = notFoldedItem.Item.Id,
                    Amount = 1,
                };
                heroDal.Items.Add(newItemCell);
                _context.HeroItemCells.Add(newItemCell);
            }
        }
    }

    private async Task<HeroDal> GetWithItemsAsync(Guid id)
    {
        var hero = await _context.Heroes
            .Include(h => h.Items)
            .ThenInclude(ic => ic.Item)
            .ThenInclude(i => i!.Effects)
            .Include(h => h.Items)
            .ThenInclude(ic => ic.Item)
            .ThenInclude(i => i!.AllowedSlots)
            .Include(h => h.EquippedSlots)
            .ThenInclude(es => es.HeroItemCell)
            .ThenInclude(ic => ic!.Item)
            .ThenInclude(i => i!.Effects)
            .FirstAsync(h => h.Id == id);
        return hero;
    }

    #endregion private methods
}
