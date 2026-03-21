
using FastArena.Core.Domain.Activities;

namespace FastArena.Core.Interfaces.Storages;

public interface IActivitySessionStorage
{
    Task<ActivitySession?> GetByHeroIdAsync(Guid heroId);
    Task<ActivitySession?> GetByIdAsync(Guid id);
    Task<ActivitySession> CreateAsync(ActivitySession session);
    Task<ActivitySession> UpdateAsync(ActivitySession session);
    Task DeleteAsync(Guid id);
}
