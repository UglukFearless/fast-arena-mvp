
using FastArena.Core.Domain.Activities;
using FastArena.Core.Interfaces.App;
using FastArena.Core.Interfaces.Storages;
using System.Collections.Concurrent;

namespace FastArena.Core.Services;

public class ActivityStateService : IActivityStateService
{
    private readonly IActivitySessionStorage _activitySessionStorage;
    private readonly IUserStorage _userStorage;

    private readonly ConcurrentDictionary<Guid, SemaphoreSlim> _sessionLocks = new();

    public ActivityStateService(IActivitySessionStorage activitySessionStorage, IUserStorage userStorage)
    {
        _activitySessionStorage = activitySessionStorage;
        _userStorage = userStorage;
    }

    public async Task<bool> IsBusyAsync(Guid userId)
    {
        var user = await _userStorage.GetAsync(userId);

        if (user == null)
        {
            throw new Exception($"User with id {userId} is not found!");
        }

        if (!user.SelectedHeroId.HasValue)
        {
            return false;
        }

        var activitySession = await _activitySessionStorage.GetByHeroIdAsync(user.SelectedHeroId.Value);

        return activitySession == null ? false : true;
    }

    public async Task CreateStateAsync(ActivitySession session)
    {
        var activitySession = await _activitySessionStorage.CreateAsync(session);
    }

    public async Task UpdateStateAsync(ActivitySession session)
    {
        var activitySession = await _activitySessionStorage.UpdateAsync(session);
    }

    public async Task<ActivitySession?> GetByHeroIdAsync(Guid heroId)
    {
        return await _activitySessionStorage.GetByHeroIdAsync(heroId);
    }

    public async Task<SemaphoreSlim> GetLockerForSessionIdAsync(Guid sessionId)
    {
        return await Task.FromResult(_sessionLocks.GetOrAdd(sessionId, _ => new SemaphoreSlim(1, 1)));
    }

    public async Task<ActivitySession?> GetAsync(Guid id)
    {
        return await _activitySessionStorage.GetByIdAsync(id);
    }

    public async Task CleanTheStateAsync(Guid id)
    {
        await _activitySessionStorage.DeleteAsync(id);
    }

    public async Task CleanTheLocker(Guid id)
    {
        var unlockResult = _sessionLocks.TryRemove(id, out _);
    }
}
