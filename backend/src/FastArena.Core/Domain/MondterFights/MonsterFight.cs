
using FastArena.Core.Domain.Activities.Actions;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Monsters;

namespace FastArena.Core.Domain.MondterFights;

public class MonsterFight
{
    public required Hero Hero { get; set; }
    public required Monster Monster { get; set; }
    public required Dictionary<int, MonsterFightActionState> State { get; set; }
    public MonsterFightReward? Reward { get; set; }
}
