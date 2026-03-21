
using FastArena.Core.Domain.Activities;

namespace FastArena.Core.Interfaces.App;

public interface IActivitySessionService
{
    Task<ActivitySession> StartActivityAsync(Guid activityId, Guid userId);
    Task<ActivitySession> GetCurrentActivityByUserIdAsync(Guid userId);
    Task<bool> HasNextAsync(ActivitySession activitySession);
    Task<ActivitySession> GoNextAsync(ActivitySession activitySession);
}
