
using FastArena.Core.Domain.Monsters;

namespace FastArena.Core.Interfaces.App;

public interface IMonsterService
{
    Task<ICollection<MonsterMold>> GetAllMoldsAsync();
    Task<MonsterMold> GetRandomMoldForRankAsync(int rank);
    Task<Monster> CreateFromMoldAsync(int level, MonsterMold mold);
}
