using FastArena.Core.Domain.Activities.Actions;
using FastArena.Core.Domain.Effects;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Monsters;
using FastArena.Core.Interfaces.Effects;

namespace FastArena.Core.Services.Effects;

public class IncomingStrikeFullBlockEffectHandler : IEffectHandler
{
    public EffectType EffectType => EffectType.INCOMING_STRIKE_FULL_BLOCK;

    public void OnRoundStart(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster) { }

    public void OnStrikeClaimed(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster) { }

    public void OnIncomingStrikeConfirmed(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster)
    {
        if (state.StrikeBlocked)
        {
            return;
        }

        if (effect.Magnitude <= 0)
        {
            return;
        }

        var roll = Random.Shared.Next(1, 101);
        if (roll <= effect.ChancePercent)
        {
            state.StrikeBlocked = true;
            effect.Magnitude -= 1;
        }
    }

    public void OnBeforeDamageCommit(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster) { }

    public void OnRoundEnd(ActiveEffect effect, MonsterFightActionState state, Hero hero, Monster monster) { }

    public ActiveEffect Stack(ActiveEffect existing, EffectDefinition newDefinition)
    {
        return existing;
    }
}