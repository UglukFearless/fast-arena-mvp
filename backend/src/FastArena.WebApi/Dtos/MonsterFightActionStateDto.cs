
using FastArena.Core.Domain.Activities.Actions;

namespace FastArena.WebApi.Dtos;

public class MonsterFightActionStateDto
{
    public int HeroHealth { get; set; }
    public int HeroAbility { get; set; }
    public int? HeroDiceRoll { get; set; }
    public int MonsterHealth { get; set; }
    public int MonsterAbility { get; set; }
    public int? MonsterDiceRoll { get; set; }
    public MonsterFightActionStateResult? Result { get; set; }
    public HashSet<HeroActVariant> ActVariants { get; set; }
}
