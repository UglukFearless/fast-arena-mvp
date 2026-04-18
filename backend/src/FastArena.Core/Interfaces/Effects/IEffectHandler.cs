using FastArena.Core.Domain.Activities.Actions;
using FastArena.Core.Domain.Effects;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Monsters;

namespace FastArena.Core.Interfaces.Effects;

/// <summary>
/// Contract for effect-type-specific handler logic during combat rounds.
/// Each EffectType maps to one IEffectHandler implementation.
/// Handlers are stateless; all state lives in the ActiveEffect instance.
/// </summary>
public interface IEffectHandler
{
    EffectType EffectType { get; }

    /// <summary>
    /// Called at round start, before initiative.
    /// Apply immediate effects (e.g., heal potion restores HP immediately).
    /// </summary>
    void OnRoundStart(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster);
    
    /// <summary>
    /// Called after initiative rolls and advantage calculation.
    /// Apply ability override or other pre-strike modifiers.
    /// </summary>
    void OnStrikeClaimed(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster);
    
    /// <summary>
    /// Called after hit zone is determined but before damage calculation.
    /// Apply strike power modifiers and other power-level adjustments.
    /// </summary>
    void OnPowerModifiers(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster);
    
    /// <summary>
    /// Called after damage is committed to HP.
    /// Apply post-hit effects and state updates.
    /// </summary>
    void OnRoundEnd(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster);
    
    /// <summary>
    /// Merges a new effect activation with an existing active effect of the same type.
    /// Implements the stacking rule specific to this effect type.
    /// Returns the merged/updated ActiveEffect that should remain active.
    /// </summary>
    ActiveEffect Stack(ActiveEffect existing, EffectDefinition newDefinition);
}
