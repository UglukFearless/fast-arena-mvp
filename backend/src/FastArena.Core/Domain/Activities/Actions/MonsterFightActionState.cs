
namespace FastArena.Core.Domain.Activities.Actions;

public class MonsterFightActionState : ActivityActionState
{
    public int HeroHealth { get; set; }
    public int HeroAbility { get; set; }
    public int? HeroDiceRoll { get; set; }
    public int MonsterHealth { get; set; }
    public int MonsterAbility { get; set;}
    public int? MonsterDiceRoll { get;set; }
    public MonsterFightActionStateResult? Result { get; set; }
    public HashSet<HeroActVariant> ActVariants { get; set; }
}

public class MonsterFightActionStateResult
{
    public required string ResultText { get; set; }
    public required MonsterFightActionStateResultType ResultType { get; set; }
}

public enum MonsterFightActionStateResultType
{
    STRIKE_BY_HERO,
    STRIKE_BY_MONSTER,
    DRAW
}

public enum HeroActVariant
{
    ATTACK,
    FINALIZE,
}
