
using FastArena.Core.Domain.Activities;
using FastArena.Core.Services;

namespace FastArena.Core.Interfaces.App;

public interface IActivityStateService
{
    Task<bool> IsBusyAsync(Guid userId);
    Task CreateStateAsync(ActivitySession session);
    Task UpdateStateAsync(ActivitySession session);
    Task<ActivitySession?> GetByHeroIdAsync(Guid heroId);
    Task<ActivitySession?> GetAsync(Guid id);
    Task<SemaphoreSlim> GetLockerForSessionIdAsync(Guid sessionId);
    Task CleanTheStateAsync(Guid id);
    Task CleanTheLocker(Guid id);
}