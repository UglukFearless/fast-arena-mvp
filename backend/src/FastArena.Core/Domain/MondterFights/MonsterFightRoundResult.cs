using FastArena.Core.Domain.Activities.Actions;

namespace FastArena.Core.Domain.MondterFights;

public class MonsterFightRoundResult
{
    public bool ShoudGoNext { get; set; }
    public int? StateOrder { get; set; }
    public MonsterFightActionState? State { get; set; }
    public MonsterFightReward? Reward { get; set; }
}
