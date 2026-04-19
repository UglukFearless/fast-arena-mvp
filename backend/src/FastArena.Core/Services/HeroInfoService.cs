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
        var equippedCellIds = hero.EquippedSlots?
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
                ? GetFixedPocketSlots(hero)
                : new List<HeroEquippedSlot>(),
        };
    }

    private static List<HeroEquippedSlot> GetFixedPocketSlots(Hero hero)
    {
        var slotsByType = hero.EquippedSlots?
            .Where(s => IsPocketSlot(s.Slot))
            .ToDictionary(s => s.Slot)
            ?? new Dictionary<EquipmentSlotType, HeroEquippedSlot>();

        var pocketOrder = new[]
        {
            EquipmentSlotType.POCKET_1,
            EquipmentSlotType.POCKET_2,
            EquipmentSlotType.POCKET_3,
        };

        return pocketOrder.Select(slot =>
        {
            if (slotsByType.TryGetValue(slot, out var existed))
            {
                return existed;
            }

            return new HeroEquippedSlot
            {
                HeroId = hero.Id,
                Slot = slot,
                HeroItemCellId = null,
                HeroItemCell = null,
            };
        }).ToList();
    }

    private static bool IsPocketSlot(EquipmentSlotType slot)
    {
        return slot == EquipmentSlotType.POCKET_1
            || slot == EquipmentSlotType.POCKET_2
            || slot == EquipmentSlotType.POCKET_3;
    }
}
