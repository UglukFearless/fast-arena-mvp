using FastArena.Core.Domain.Activities.Actions;
using FastArena.Core.Domain.Effects;
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
            ActVariants = GetInitHeroActVariantsAsync(hero),
            ActiveEffects = new List<ActiveEffect>(),
            PocketItems = GetPocketItems(hero),
        };

        return await Task.FromResult(state);
    }

    private HashSet<HeroActVariant> GetInitHeroActVariantsAsync(Hero hero)
    {
        var result = new HashSet<HeroActVariant> { HeroActVariant.ATTACK };
        if (GetPocketItems(hero).Count > 0)
        {
            result.Add(HeroActVariant.USE_ITEM);
        }

        return result;
    }

    private static List<HeroItemCell> GetPocketItems(Hero hero)
    {
        return hero.EquippedSlots?
            .Where(s => s.HeroItemCell != null &&
                (s.Slot == EquipmentSlotType.POCKET_1 ||
                 s.Slot == EquipmentSlotType.POCKET_2 ||
                 s.Slot == EquipmentSlotType.POCKET_3))
            .Select(s => s.HeroItemCell!)
            .ToList() ?? new List<HeroItemCell>();
    }
}
