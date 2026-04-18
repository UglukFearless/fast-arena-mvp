
using FastArena.Core.Domain.Activities.Actions;
using FastArena.Core.Domain.MondterFights;
using FastArena.Core.Models;

namespace FastArena.Core.Interfaces.App;

public interface IMonsterFightService
{
    Task<MonsterFight> GetByUserIdAsync(Guid userId);
    Task<MonsterFightRoundResult> CalcRoundAsync(MonsterFightActionPayload payload, Guid userId);
}
