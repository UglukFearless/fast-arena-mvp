
using FastArena.Core.Domain.MondterFights;
using FastArena.Core.Models;

namespace FastArena.Core.Interfaces.Storages;

public interface IMonsterFightResultStorage
{
    Task<MonsterFightResult> CreateAsync(MonsterFightResultCreationModel model);
    Task<MonsterFightResult> GetAsync(Guid id);
    Task<ICollection<MonsterFightResult>> GetByHeroIdAsync(Guid heroId);
}
