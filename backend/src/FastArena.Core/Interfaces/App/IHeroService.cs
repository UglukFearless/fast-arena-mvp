using FastArena.Core.Domain.Heroes;
using FastArena.Core.Models;

namespace FastArena.Core.Interfaces.App;

public interface IHeroService
{
    Task<ICollection<Hero>> GetAllByUserIdAsync(Guid userId);
    Task<ICollection<Hero>> GetAllWithInfoByUserIdAsync(Guid userId);
    Task<ICollection<Hero>> GetAllWithInfoAsync();
    Task<Hero> GetSelectedByUserIdAsync(Guid userId);
    Task<Hero> CreateAsync(HeroCreationModel model);
    Task SelectForUserAsync(Guid id, Guid userId);
    Task UnselectForUserAsync(Guid userId);
    Task UnselectForUserForceAsync(Guid userId);
    Task<Hero> GetAsync(Guid id);
    Task KillTheHeroAsync(Guid id);
    Task GiveItemsToHeroAsync(Guid heroId, ICollection<GivenItem> items);
    Task<HeroItemCell> ConsumePocketItemForFightAsync(Guid heroId, Guid heroItemCellId);
    Task IncreaseExperienceAsync(int experience, Guid heroId);
}
