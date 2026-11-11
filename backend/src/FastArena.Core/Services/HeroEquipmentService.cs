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

        var requiredKeys = ResolveRequiredSlotKeys(heroItemCell.Item);
        if (requiredKeys.Count == 0)
        {
            throw new ActionDeniedException("Selected item cannot be equipped.");
        }

        var eligibleSlots = hero.Slots?
            .Where(s => requiredKeys.Contains(s.SlotKey))
            .ToList()
            ?? new List<HeroSlot>();

        if (eligibleSlots.Count == 0)
        {
            throw new ActionDeniedException("Hero has no slots for this item.");
        }

        var slotsToOccupy = new List<HeroSlot>();

        foreach (var group in requiredKeys.GroupBy(key => key))
        {
            var groupFreeSlots = eligibleSlots
                .Where(s => s.SlotKey == group.Key && !s.HeroItemCellId.HasValue)
                .OrderBy(s => s.Id)
                .ToList();

            if (groupFreeSlots.Count < group.Count())
            {
                throw new ActionDeniedException($"No free slot for '{group.Key}'.");
            }

            slotsToOccupy.AddRange(groupFreeSlots.Take(group.Count()));
        }

        await _heroEquipmentStorage.EquipItemAsync(
            hero.Id,
            heroItemCellId,
            slotsToOccupy.Select(s => s.Id).ToList());
    }

    public async Task UnequipAsync(Guid userId, Guid heroItemCellId)
    {
        var hero = await EnsureHeroCanManageEquipmentAsync(userId);

        var isEquipped = hero.Slots?.Any(s => s.HeroItemCellId == heroItemCellId) == true;
        if (!isEquipped)
        {
            throw new ActionDeniedException("Selected item is not equipped.");
        }

        await _heroEquipmentStorage.UnequipItemAsync(hero.Id, heroItemCellId);
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

    private static List<string> ResolveRequiredSlotKeys(Item item)
    {
        return item.OccupiedSlots?
            .Select(s => s.SlotKey)
            .ToList()
            ?? new List<string>();
    }
}
