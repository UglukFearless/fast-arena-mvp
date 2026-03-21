using FastArena.Core.Domain.Monsters;

namespace FastArena.Core.Interfaces.Storages;

public interface IMonsterMoldStorage
{
    Task<MonsterMold> GetAsync(Guid id);
    Task<ICollection<MonsterMold>> GetAllAsync();
    Task<ICollection<MonsterMold>> GetFiltered(int maxRank);
}
