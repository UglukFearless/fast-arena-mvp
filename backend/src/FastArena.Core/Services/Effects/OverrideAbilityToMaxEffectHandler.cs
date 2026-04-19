using FastArena.Core.Domain.Activities.Actions;
using FastArena.Core.Domain.Effects;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Monsters;
using FastArena.Core.Interfaces.Effects;

namespace FastArena.Core.Services.Effects;

/// <summary>
/// Handler for OVERRIDE_ABILITY_TO_MAX effect.
/// Applies ability override at OnStrikeClaimed (after initiative roll and advantage calc).
/// Overrides hero's ability to floor(MaxHP / 10) regardless of current HP.
/// Stacking rule: sum remaining durations.
/// </summary>
public class OverrideAbilityToMaxEffectHandler : IEffectHandler
{
    public EffectType EffectType => EffectType.OVERRIDE_ABILITY_TO_MAX;

    public void OnRoundStart(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster) { }
    
    public void OnStrikeClaimed(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster)
    {
        state.HeroAbility = hero.MaxHealth / 10;
    }
    
    public void OnPowerModifiers(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster) { }
    
    public void OnRoundEnd(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster)
    {
        state.HeroAbility = hero.MaxHealth / 10;
    }
    
    public ActiveEffect Stack(ActiveEffect existing, EffectDefinition newDefinition)
    {
        // Stacking rule: sum remaining durations
        existing.RemainingRounds += newDefinition.DurationRounds;
        existing.StackCount++;
        
        return existing;
    }
}
