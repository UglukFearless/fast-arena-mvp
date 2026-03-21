using FastArena.Core.Domain.Activities.Actions;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.MondterFights;
using FastArena.Core.Domain.Monsters;

namespace FastArena.Core.Domain.Activities.Datas;

public class MonsterFightData : ActivityActionCaseData
{
    public required Monster Monster { get; set; }
    public MonsterFightReward? Reward { get; set; }

    public override async Task<ActivityActionState> BuildInitStateAsync(Hero hero, ActivityActionCase activityActionCase)
    {
        var state = new MonsterFightActionState()
        {
            HeroHealth = hero.MaxHealth,
            HeroAbility = (int)hero.MaxHealth / 10,
            HeroDiceRoll = null,
            MonsterHealth = Monster.MaxHealth,
            MonsterAbility = (int)Monster.MaxHealth / 10,
            MonsterDiceRoll = null,
            Result = null,
            ActVariants = GetInitHeroActVariantsAsync(),
        };

        return await Task.FromResult(state);
    }

    private HashSet<HeroActVariant> GetInitHeroActVariantsAsync()
    {
        return new HashSet<HeroActVariant> { HeroActVariant.ATTACK };
    }
}
