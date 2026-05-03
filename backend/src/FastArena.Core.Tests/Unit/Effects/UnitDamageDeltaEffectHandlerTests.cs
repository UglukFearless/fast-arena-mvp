using FastArena.Core.Domain.Effects;
using FastArena.Core.Services.Effects;
using FastArena.Core.Tests.Support;

namespace FastArena.Core.Tests;

public class UnitDamageDeltaEffectHandlerTests
{
    [Fact]
    public void OnBeforeDamageCommit_AddsMagnitudeToUnitDamageModifier()
    {
        var handler = new UnitDamageDeltaEffectHandler();
        var hero = EffectTestData.CreateHero();
        var monster = EffectTestData.CreateMonster();
        var state = EffectTestData.CreateState();
        state.UnitDamageModifier = 1;
        var effect = EffectTestData.CreateActiveEffect(EffectType.UNIT_DAMAGE_DELTA, remainingRounds: 0, magnitude: 2);
        effect.LifetimeType = EffectLifetimeType.Persistent;
        effect.SourceType = EffectSourceType.Equipment;

        handler.OnBeforeDamageCommit(effect, state, hero, monster);

        Assert.Equal(3, state.UnitDamageModifier);
    }
}