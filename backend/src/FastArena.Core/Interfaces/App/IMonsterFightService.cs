
using FastArena.Core.Domain.Activities.Actions;
using FastArena.Core.Domain.MondterFights;

namespace FastArena.Core.Interfaces.App;

public interface IMonsterFightService
{
    Task<MonsterFight> GetByUserIdAsync(Guid userId);
    Task<MonsterFightRoundResult> CalcRoundAsync(HeroActVariant heroActVariant, Guid userId);
}
