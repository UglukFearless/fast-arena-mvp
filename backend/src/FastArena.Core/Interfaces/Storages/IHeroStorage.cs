using FastArena.Core.Domain.Heroes;
using FastArena.Core.Models;

namespace FastArena.Core.Interfaces.Storages;

public interface IHeroStorage
{
    Task<ICollection<Hero>> GetAllByUserIdAsync(Guid userId);
    Task<ICollection<Hero>> GetAllWithIncludesAsync();
    Task<ICollection<Hero>> GetAllWithIncludesByUserIdAsync(Guid userId);
    Task<Hero> GetAsync(Guid id);
    Task<Hero> CreateAsync(HeroCreationModel model);
    Task<List<HeroItemCell>> GiveItemsToHeroAsync(Guid heroId, ICollection<GivenItem> items);
    Task<Hero> UpdateHeroAsync(Hero hero);
}
