using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Items;
using FastArena.Core.Exceptions;
using FastArena.Core.Interfaces.App;
using FastArena.Core.Interfaces.Storages;

namespace FastArena.Core.Services;

public class HeroEquipmentService : IHeroEquipmentService
{
    private readonly IUserStorage _userStorage;
    private readonly IActivityStateService _activityStateService;
    private readonly IHeroStorage _heroStorage;
    private readonly IHeroEquipmentStorage _heroEquipmentStorage;

    public HeroEquipmentService(
        IUserStorage userStorage,
        IActivityStateService activityStateService,
        IHeroStorage heroStorage,
        IHeroEquipmentStorage heroEquipmentStorage)
    {
        _userStorage = userStorage;
        _activityStateService = activityStateService;
        _heroStorage = heroStorage;
        _heroEquipmentStorage = heroEquipmentStorage;
    }

    public async Task EquipAsync(Guid userId, Guid heroItemCellId)
    {
        var hero = await EnsureHeroCanManageEquipmentAsync(userId);

        var heroItemCell = hero.Items?.FirstOrDefault(i => i.Id == heroItemCellId);
        if (heroItemCell == null)
        {
            throw new ActionDeniedException("Hero does not own the selected item.");
        }

        if (heroItemCell.Item == null)
        {
            throw new ActionDeniedException("Selected item is missing.");
        }

        var item = heroItemCell.Item;
        var allowedSlots = ResolveAllowedSlots(item);
        if (allowedSlots.Count == 0)
        {
            throw new ActionDeniedException("Selected item cannot be equipped.");
        }

        var isTwoHandedWeapon = IsTwoHandedWeapon(item);
        var hasTwoHandedWeaponEquipped = HasTwoHandedWeaponEquipped(hero);

        if (isTwoHandedWeapon && IsAnyHandOccupied(hero))
        {
            throw new ActionDeniedException("Two-handed weapon requires both hands to be free.");
        }

        if (!isTwoHandedWeapon && hasTwoHandedWeaponEquipped && IsHandItemType(item.Type))
        {
            throw new ActionDeniedException("Cannot equip hand item while a two-handed weapon is equipped.");
        }

        var occupiedSlots = hero.EquippedSlots?
            .Where(s => s.HeroItemCellId.HasValue)
            .Select(s => s.Slot)
            .ToHashSet()
            ?? new HashSet<EquipmentSlotType>();

        var freeSlot = allowedSlots.FirstOrDefault(slot => !occupiedSlots.Contains(slot));
        if (occupiedSlots.Contains(freeSlot) || !allowedSlots.Contains(freeSlot))
        {
            throw new ActionDeniedException("All compatible slots are occupied.");
        }

        await _heroEquipmentStorage.EquipItemToSlotAsync(hero.Id, heroItemCellId, freeSlot);
    }

    public async Task UnequipAsync(Guid userId, EquipmentSlotType slot)
    {
        var hero = await EnsureHeroCanManageEquipmentAsync(userId);

        var slotState = hero.EquippedSlots?.FirstOrDefault(s => s.Slot == slot);
        if (slotState == null || !slotState.HeroItemCellId.HasValue)
        {
            throw new ActionDeniedException("Selected equipment slot is already empty.");
        }

        await _heroEquipmentStorage.UnequipItemFromSlotAsync(hero.Id, slot);
    }

    private async Task<Hero> EnsureHeroCanManageEquipmentAsync(Guid userId)
    {
        var user = await _userStorage.GetAsync(userId);

        if (!user.SelectedHeroId.HasValue)
        {
            throw new EntityNotFoundException("No hero is selected");
        }

        var isBusy = await _activityStateService.IsBusyAsync(userId);
        if (isBusy)
        {
            throw new ActionDeniedException("Hero is on adventure and cannot change equipment");
        }

        var hero = await _heroStorage.GetAsync(user.SelectedHeroId.Value);
        if (hero.IsAlive == HeroAliveState.DEAD)
        {
            throw new ActionDeniedException("Dead hero cannot change equipment");
        }

        return hero;
    }

    private static List<EquipmentSlotType> ResolveAllowedSlots(Item item)
    {
        var allowedSlots = item.AllowedSlots?
            .Select(s => s.Slot)
            .Distinct()
            .ToList()
            ?? new List<EquipmentSlotType>();

        return allowedSlots;
    }

    private static bool IsTwoHandedWeapon(Item item)
    {
        if (item.Type != ItemType.WEAPON)
        {
            return false;
        }

        var slots = item.AllowedSlots?
            .Select(s => s.Slot)
            .Distinct()
            .ToHashSet()
            ?? new HashSet<EquipmentSlotType>();

        return slots.Contains(EquipmentSlotType.RIGHT_HAND) && slots.Contains(EquipmentSlotType.LEFT_HAND);
    }

    private static bool HasTwoHandedWeaponEquipped(Hero hero)
    {
        var heroItemsByCellId = hero.Items?
            .Where(i => i.Item != null)
            .ToDictionary(i => i.Id)
            ?? new Dictionary<Guid, HeroItemCell>();

        var equippedItemIds = hero.EquippedSlots?
            .Where(s => s.HeroItemCellId.HasValue)
            .Select(s => s.HeroItemCellId!.Value)
            .ToList()
            ?? new List<Guid>();

        foreach (var cellId in equippedItemIds)
        {
            if (!heroItemsByCellId.TryGetValue(cellId, out var cell) || cell.Item == null)
            {
                continue;
            }

            if (IsTwoHandedWeapon(cell.Item))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsAnyHandOccupied(Hero hero)
    {
        return hero.EquippedSlots?.Any(s =>
            s.HeroItemCellId.HasValue &&
            (s.Slot == EquipmentSlotType.RIGHT_HAND || s.Slot == EquipmentSlotType.LEFT_HAND)) == true;
    }

    private static bool IsHandItemType(ItemType itemType)
    {
        return itemType == ItemType.WEAPON || itemType == ItemType.SHIELD;
    }
}