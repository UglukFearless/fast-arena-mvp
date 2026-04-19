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

        if (heroItemCell.Item.Type != ItemType.POTION)
        {
            throw new ActionDeniedException("Only potion items can be equipped in the current MVP step.");
        }

        var allowedPocketSlots = ResolveAllowedPocketSlots(heroItemCell.Item);
        if (allowedPocketSlots.Count == 0)
        {
            throw new ActionDeniedException("Selected item cannot be placed into pockets.");
        }

        var occupiedSlots = hero.EquippedSlots?
            .Where(s => s.HeroItemCellId.HasValue)
            .Select(s => s.Slot)
            .ToHashSet()
            ?? new HashSet<EquipmentSlotType>();

        var freeSlot = allowedPocketSlots.FirstOrDefault(slot => !occupiedSlots.Contains(slot));
        if (occupiedSlots.Contains(freeSlot) || !allowedPocketSlots.Contains(freeSlot))
        {
            throw new ActionDeniedException("All pockets are occupied.");
        }

        await _heroEquipmentStorage.EquipItemToSlotAsync(hero.Id, heroItemCellId, freeSlot);
    }

    public async Task UnequipAsync(Guid userId, EquipmentSlotType slot)
    {
        var hero = await EnsureHeroCanManageEquipmentAsync(userId);

        if (!IsPocketSlot(slot))
        {
            throw new ActionDeniedException("Only pocket slots can be changed in the current MVP step.");
        }

        var slotState = hero.EquippedSlots?.FirstOrDefault(s => s.Slot == slot);
        if (slotState == null || !slotState.HeroItemCellId.HasValue)
        {
            throw new ActionDeniedException("Selected pocket slot is already empty.");
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

    private static List<EquipmentSlotType> ResolveAllowedPocketSlots(Item item)
    {
        var allowedSlots = item.AllowedSlots?
            .Select(s => s.Slot)
            .Where(IsPocketSlot)
            .Distinct()
            .ToList()
            ?? new List<EquipmentSlotType>();

        return allowedSlots;
    }

    private static bool IsPocketSlot(EquipmentSlotType slot)
    {
        return slot == EquipmentSlotType.POCKET_1
            || slot == EquipmentSlotType.POCKET_2
            || slot == EquipmentSlotType.POCKET_3;
    }
}