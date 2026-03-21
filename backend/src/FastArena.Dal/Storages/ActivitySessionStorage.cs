using FastArena.Core.Domain.Activities;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Interfaces.Storages;
using System.Collections.Concurrent;

namespace FastArena.Dal.Storages;

public class ActivitySessionStorage : IActivitySessionStorage
{
    // Yes, it's a memory storage, you're right!
    private readonly ConcurrentDictionary<Guid, ActivitySession> _sessionStorage;

    public ActivitySessionStorage()
    {
        _sessionStorage = new ConcurrentDictionary<Guid, ActivitySession>();
    }

    public async Task<ActivitySession> CreateAsync(ActivitySession session)
    {
        var existingSession = await GetByIdAsync(session.Id) ?? await GetByHeroIdAsync(session.HeroId);

        if ( existingSession != null)
        {
            throw new Exception(
                $"There is already s session with id {session.Id} or for heroId {session.HeroId}"
            );
        }

        var res = _sessionStorage.TryAdd(session.Id, session);
        if (!res)
        {
            throw new Exception("Activity session adding attempt was failed.");
        }
        return session;
    }

    public async Task DeleteAsync(Guid id)
    {
        var existingSession = await GetByIdAsync(id);

        if (existingSession == null)
        {
            throw new Exception(
                $"There is no any session with id {id}"
            );
        }

        var res =_sessionStorage.TryRemove(id, out _);
        if (!res)
        {
            throw new Exception("Activity session removing attempt was failed.");
        }
    }

    public async Task<ActivitySession> UpdateAsync(ActivitySession session)
    {
        var existingSession = await GetByIdAsync(session.Id);

        if (existingSession == null)
        {
            throw new Exception(
                $"There is no any session with id {session.Id}"
            );
        }

        var res = _sessionStorage.AddOrUpdate(session.Id, session, (key, oldSession) => session);

        return res;
    }

    public Task<ActivitySession?> GetByHeroIdAsync(Guid heroId)
    {
        var sessionPair = _sessionStorage.FirstOrDefault(p => p.Value.HeroId == heroId);

        if (sessionPair.Key == Guid.Empty)
            return Task.FromResult(null as ActivitySession);

        return Task.FromResult(sessionPair.Value);
    }

    public Task<ActivitySession?> GetByIdAsync(Guid id)
    {
        var sessionPair = _sessionStorage.FirstOrDefault(p => p.Value.Id == id);

        if (sessionPair.Key == Guid.Empty)
            return Task.FromResult(null as ActivitySession);

        return Task.FromResult(sessionPair.Value);
    }
}
