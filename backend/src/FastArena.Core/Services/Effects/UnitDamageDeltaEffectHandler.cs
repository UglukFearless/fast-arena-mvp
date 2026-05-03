using FastArena.Core.Domain.Activities.Actions;
using FastArena.Core.Domain.Effects;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Monsters;
using FastArena.Core.Interfaces.Effects;

namespace FastArena.Core.Services.Effects;

public class UnitDamageDeltaEffectHandler : IEffectHandler
{
    public EffectType EffectType => EffectType.UNIT_DAMAGE_DELTA;

    public void OnRoundStart(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster) { }

    public void OnStrikeClaimed(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster) { }

    public void OnIncomingStrikeConfirmed(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster) { }

    public void OnBeforeDamageCommit(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster)
    {
        state.UnitDamageModifier += effect.Magnitude;
    }

    public void OnRoundEnd(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster) { }

    public ActiveEffect Stack(ActiveEffect existing, EffectDefinition newDefinition)
    {
        return existing;
    }
}