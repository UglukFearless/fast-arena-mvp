using FastArena.Core.Domain;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Exceptions;
using FastArena.Core.Interfaces.App;
using FastArena.Core.Interfaces.Storages;
using FastArena.Core.Models;

namespace FastArena.Core.Services;

public class HeroService : IHeroService
{
    private readonly IHeroStorage _heroStorage;
    private readonly IUserStorage _userStorage;
    private readonly IItemStorage _itemStorage;
    private readonly IHeroProgressService _heroProgressService;
    private readonly IActivityStateService _activityStateService;
    private readonly IHeroEquipmentStorage _heroEquipmentStorage;

    public HeroService(
        IHeroStorage heroStorage, 
        IUserStorage userStorage,
        IItemStorage itemStorage,
        IHeroProgressService heroProgressService,
        IActivityStateService activityStateService,
        IHeroEquipmentStorage heroEquipmentStorage)
    {
        _heroStorage = heroStorage;
        _userStorage = userStorage;
        _itemStorage = itemStorage;
        _heroProgressService = heroProgressService;
        _activityStateService = activityStateService;
        _heroEquipmentStorage = heroEquipmentStorage;
    }

    public async Task<Hero> CreateAsync(HeroCreationModel model)
    {
        var hero = await _heroStorage.CreateAsync(model);
        var item = await _itemStorage.GetBaseMoneyItemAsync();
        var heroItems = await _heroStorage.GiveItemsToHeroAsync(hero.Id, [
            new GivenItem
            {
                Item = item,
                Amount = 10,
            }
        ]);
        hero.Items = heroItems;
        return await AddProgressInfo(hero);
    }

    public async Task<ICollection<Hero>> GetAllByUserIdAsync(Guid userId)
    {
        var heroes = await _heroStorage.GetAllByUserIdAsync(userId);
        return await AddProgressInfo(heroes.ToList());
    }

    public async Task<ICollection<Hero>> GetAllWithInfoAsync()
    {
        var heroes = await _heroStorage.GetAllWithIncludesAsync();
        return await AddProgressInfo(heroes.ToList());
    }

    public async Task<ICollection<Hero>> GetAllWithInfoByUserIdAsync(Guid userId)
    {
        var heroes = await _heroStorage.GetAllWithIncludesByUserIdAsync(userId);
        return await AddProgressInfo(heroes.ToList());
    }

    public async Task<Hero> GetAsync(Guid id)
    {
        var hero = await _heroStorage.GetAsync(id);
        return hero;
    }

    public async Task<Hero> GetSelectedByUserIdAsync(Guid userId)
    {
        var user = await _userStorage.GetAsync(userId);
        if (!user.SelectedHeroId.HasValue)
            return null;

        var hero = await _heroStorage.GetAsync(user.SelectedHeroId.Value);
        return await AddProgressInfo(hero);
    }

    public async Task GiveItemsToHeroAsync(Guid heroId, ICollection<GivenItem> items)
    {
        await _heroStorage.GiveItemsToHeroAsync(heroId, items);
    }

    public async Task<HeroItemCell> ConsumePocketItemForFightAsync(Guid heroId, Guid heroItemCellId)
    {
        return await _heroEquipmentStorage.ConsumePocketItemAsync(heroId, heroItemCellId);
    }

    public async Task IncreaseExperienceAsync(int experience, Guid heroId)
    {
        var hero = await _heroStorage.GetAsync(heroId);
        hero.Experience += experience;

        var progressInfo = await _heroProgressService.GetInfoByLevelAsync(hero.Level);

        while (progressInfo.NextAmound <= hero.Experience)
        {
            hero.Level += 1;
            hero.MaxHealth += 10;

            progressInfo = await _heroProgressService.GetInfoByLevelAsync(hero.Level);
        }

        await _heroStorage.UpdateHeroAsync(hero);
    }

    public async Task KillTheHeroAsync(Guid id)
    {
        var hero = await GetAsync(id);
        await UnselectForUserForceAsync(hero.UserId);
        hero.IsAlive = HeroAliveState.DEAD;
        await _heroStorage.UpdateHeroAsync(hero);
    }

    public async Task SelectForUserAsync(Guid id, Guid userId)
    {
        var isBusy = await _activityStateService.IsBusyAsync(userId);

        if (isBusy)
            throw new ActionDeniedException("Action is denied. User is busy.");

        await _userStorage.SelectHeroAsync(userId, id);
    }

    public async Task UnselectForUserAsync(Guid userId)
    {
        var isBusy = await _activityStateService.IsBusyAsync(userId);

        if (isBusy)
            throw new ActionDeniedException("Action is denied. User is busy.");

        await _userStorage.UnselectHeroAsync(userId);
    }

    public async Task UnselectForUserForceAsync(Guid userId)
    {
        await _userStorage.UnselectHeroAsync(userId);
    }

    private async Task<Hero> AddProgressInfo(Hero hero)
    {
        hero.LevelProgressInfo = await _heroProgressService.GetInfoByLevelAsync(hero.Level);
        return hero;
    }

    private async Task<List<Hero>> AddProgressInfo(List<Hero> heroes) {
        return (await Task.WhenAll(
            heroes.Select(
                async h => await AddProgressInfo(h)
            )
        )).ToList();
    }
}
