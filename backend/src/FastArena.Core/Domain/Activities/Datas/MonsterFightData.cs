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

    public override async Task<ActivityActionState> BuildInitStateAsync(
        Hero hero,
        ActivityActionCase activityActionCase,
        ActivityActionState? previousState)
    {
        var previousFightState = previousState as MonsterFightActionState;
        var heroHealth = previousFightState?.HeroHealth ?? hero.MaxHealth;

        var state = new MonsterFightActionState()
        {
            HeroHealth = heroHealth,
            HeroAbility = heroHealth / 10,
            HeroDiceRoll = null,
            MonsterHealth = Monster.MaxHealth,
            MonsterAbility = (int)Monster.MaxHealth / 10,
            MonsterDiceRoll = null,
            Result = null,
            ActVariants = GetInitHeroActVariantsAsync(hero),
            ActiveEffects = GetCarriedEquipmentEffects(previousFightState),
            PocketItems = GetPocketItems(hero),
        };

        return await Task.FromResult(state);
    }

    private static List<ActiveEffect> GetCarriedEquipmentEffects(MonsterFightActionState? previousFightState)
    {
        if (previousFightState == null)
            return new List<ActiveEffect>();

        return previousFightState.ActiveEffects
            .Where(e => e.SourceType == EffectSourceType.Equipment)
            .ToList();
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
