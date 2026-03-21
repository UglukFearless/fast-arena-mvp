
using FastArena.Core.Domain.MondterFights;

namespace FastArena.Core.Interfaces.App;

public interface IMonsterFightResultService
{
    Task<MonsterFightResult> AddWinResultAsync(MonsterFight fight);
    Task<MonsterFightResult> AddLoseResultAsync(MonsterFight fight);
}
