using FastArena.Core.Domain.Effects;
using FastArena.Core.Services.Effects;
using FastArena.Core.Tests.Support;

namespace FastArena.Core.Tests;

public class StrikePowerBonusEffectHandlerTests
{
    [Fact]
    public void OnPowerModifiers_AddsMagnitudeToStrikeStrength()
    {
        var handler = new StrikePowerBonusEffectHandler();
        var hero = EffectTestData.CreateHero();
        var monster = EffectTestData.CreateMonster();
        var state = EffectTestData.CreateState(strikeStrength: 2);
        var effect = EffectTestData.CreateActiveEffect(EffectType.STRIKE_POWER_BONUS, remainingRounds: 2, magnitude: 3);

        handler.OnPowerModifiers(effect, state, hero, monster);

        Assert.Equal(5, state.StrikeStrength);
    }

    [Fact]
    public void Stack_UsesCeilingForMagnitudeAndAverageForDuration()
    {
        var handler = new StrikePowerBonusEffectHandler();
        var existing = EffectTestData.CreateActiveEffect(EffectType.STRIKE_POWER_BONUS, remainingRounds: 3, magnitude: 2);
        var newDefinition = EffectTestData.CreateEffectDefinition(EffectType.STRIKE_POWER_BONUS, durationRounds: 3, magnitude: 3);

        var merged = handler.Stack(existing, newDefinition);

        Assert.Equal(3, merged.Magnitude);
        Assert.Equal(3, merged.RemainingRounds);
        Assert.Equal(2, merged.StackCount);
    }
}