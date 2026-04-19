using FastArena.Core.Domain.Activities.Actions;
using FastArena.Core.Domain.Effects;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Monsters;
using FastArena.Core.Interfaces.Effects;

namespace FastArena.Core.Services.Effects;

/// <summary>
/// Handler for HEAL_HP effect.
/// Applies heal at OnRoundStart (before initiative).
/// Stacking rule: sum magnitudes ÷ average duration, standard round.
/// </summary>
public class HealHpEffectHandler : IEffectHandler
{
    public EffectType EffectType => EffectType.HEAL_HP;

    public void OnRoundStart(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster)
    {
        state.HeroHealth = Math.Min(state.HeroHealth + effect.Magnitude, hero.MaxHealth);
        state.HeroAbility = state.HeroHealth / 10;
    }
    
    public void OnStrikeClaimed(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster) { }
    
    public void OnPowerModifiers(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster) { }
    
    public void OnRoundEnd(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster) { }
    
    public ActiveEffect Stack(ActiveEffect existing, EffectDefinition newDefinition)
    {
        var nextCount = existing.StackCount + 1;
        var mergedMagnitude = (int)Math.Round(((existing.Magnitude * existing.StackCount) + newDefinition.Magnitude) / (double)nextCount);
        var mergedDuration = (int)Math.Round(((existing.RemainingRounds * existing.StackCount) + newDefinition.DurationRounds) / (double)nextCount);
        
        existing.Magnitude = mergedMagnitude;
        existing.RemainingRounds = mergedDuration;
        existing.StackCount = nextCount;
        
        return existing;
    }
}
