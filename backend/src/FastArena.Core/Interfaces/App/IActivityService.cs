using FastArena.Core.Domain.Activities;
using FastArena.Core.Domain.Heroes;

namespace FastArena.Core.Interfaces.App;

public interface IActivityService
{
    Task<ICollection<Activity>> GetForSelectedHeroByUserId(Guid userId);
    Task<ICollection<Activity>> GetByHero(Hero hero);
    Task<bool> IsActivityAvailableForHero(Guid activityId, Hero hero);
    Task<Activity> GetAsync(Guid id);
}
