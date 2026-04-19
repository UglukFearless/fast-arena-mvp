using FastArena.Core.Domain.Activities.Actions;
using FastArena.Core.Domain.Effects;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Monsters;
using FastArena.Core.Interfaces.Effects;

namespace FastArena.Core.Services.Effects;

/// <summary>
/// Handler for STRIKE_POWER_BONUS effect.
/// Applies strike power bonus at OnPowerModifiers (after hit zone, before damage calc).
/// Adds merged magnitude to the strike power bonus context value.
/// Stacking rule: sum magnitudes ÷ average duration, ceiling.
/// </summary>
public class StrikePowerBonusEffectHandler : IEffectHandler
{
    public EffectType EffectType => EffectType.STRIKE_POWER_BONUS;

    public void OnRoundStart(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster) { }
    
    public void OnStrikeClaimed(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster) { }
    
    public void OnPowerModifiers(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster)
    {
        state.StrikeStrength += effect.Magnitude;
    }
    
    public void OnRoundEnd(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster) { }
    
    public ActiveEffect Stack(ActiveEffect existing, EffectDefinition newDefinition)
    {
        var nextCount = existing.StackCount + 1;
        var mergedMagnitude = (int)Math.Ceiling(((existing.Magnitude * existing.StackCount) + newDefinition.Magnitude) / (double)nextCount);
        var mergedDuration = (int)Math.Round(((existing.RemainingRounds * existing.StackCount) + newDefinition.DurationRounds) / (double)nextCount);
        
        existing.Magnitude = mergedMagnitude;
        existing.RemainingRounds = mergedDuration;
        existing.StackCount = nextCount;
        
        return existing;
    }
}
