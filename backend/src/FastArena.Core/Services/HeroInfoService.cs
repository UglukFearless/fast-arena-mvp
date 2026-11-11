using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Items;
using FastArena.Core.Interfaces.App;
using FastArena.Core.Models;

namespace FastArena.Core.Services;

public class HeroInfoService : IHeroInfoService
{
    private readonly IHeroService _heroService;

    public HeroInfoService(IHeroService heroService)
    {
        _heroService = heroService;
    }

    public async Task<HeroInfoModel> GetAsync(Guid heroId, Guid requestingUserId)
    {
        var hero = await _heroService.GetAsync(heroId);

        var isInventoryVisible = hero.UserId == requestingUserId;
        var equippedCellIds = hero.Slots?
            .Where(s => s.HeroItemCellId.HasValue)
            .Select(s => s.HeroItemCellId!.Value)
            .ToHashSet()
            ?? new HashSet<Guid>();

        return new HeroInfoModel
        {
            Hero = hero,
            IsInventoryVisible = isInventoryVisible,
            MoneyAmount = isInventoryVisible
                ? hero.Items?.FirstOrDefault(i => i.Item?.Type == ItemType.MONEY)?.Amount ?? 0
                : 0,
            InventoryItems = isInventoryVisible
                ? hero.Items?.Where(i => !equippedCellIds.Contains(i.Id)).ToList() ?? new List<HeroItemCell>()
                : new List<HeroItemCell>(),
            PocketSlots = isInventoryVisible
                ? OrderInventorySlots(hero.Slots)
                : new List<HeroSlot>(),
        };
    }

    private static List<HeroSlot> OrderInventorySlots(List<HeroSlot>? slots)
    {
        return (slots ?? new List<HeroSlot>())
            .OrderBy(s => InventorySlotOrder(s.SlotKey))
            .ThenBy(s => s.Id)
            .ToList();
    }

    private static int InventorySlotOrder(string slotKey) => slotKey switch
    {
        SlotKeys.Pocket => 0,
        SlotKeys.RightHand => 1,
        SlotKeys.LeftHand => 2,
        _ => 3,
    };
}
