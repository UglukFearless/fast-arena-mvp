using FastArena.Core.Domain.Effects;
using FastArena.Core.Services.Effects;
using FastArena.Core.Tests.Support;

namespace FastArena.Core.Tests;

public class IncomingStrikeFullBlockEffectHandlerTests
{
    [Fact]
    public void OnIncomingStrikeConfirmed_WithGuaranteedChance_BlocksAndConsumesOneUse()
    {
        var handler = new IncomingStrikeFullBlockEffectHandler();
        var hero = EffectTestData.CreateHero();
        var monster = EffectTestData.CreateMonster();
        var state = EffectTestData.CreateState(strikeStrength: 2);
        var effect = EffectTestData.CreateActiveEffect(EffectType.INCOMING_STRIKE_FULL_BLOCK, remainingRounds: 0, magnitude: 2);
        effect.ChancePercent = 100;
        effect.LifetimeType = EffectLifetimeType.Persistent;
        effect.SourceType = EffectSourceType.Equipment;

        handler.OnIncomingStrikeConfirmed(effect, state, hero, monster);

        Assert.True(state.StrikeBlocked);
        Assert.Equal(1, effect.Magnitude);
    }

    [Fact]
    public void OnIncomingStrikeConfirmed_WithZeroMagnitude_DoesNothing()
    {
        var handler = new IncomingStrikeFullBlockEffectHandler();
        var hero = EffectTestData.CreateHero();
        var monster = EffectTestData.CreateMonster();
        var state = EffectTestData.CreateState(strikeStrength: 2);
        var effect = EffectTestData.CreateActiveEffect(EffectType.INCOMING_STRIKE_FULL_BLOCK, remainingRounds: 0, magnitude: 0);
        effect.ChancePercent = 100;
        effect.LifetimeType = EffectLifetimeType.Persistent;
        effect.SourceType = EffectSourceType.Equipment;

        handler.OnIncomingStrikeConfirmed(effect, state, hero, monster);

        Assert.False(state.StrikeBlocked);
        Assert.Equal(0, effect.Magnitude);
    }
}