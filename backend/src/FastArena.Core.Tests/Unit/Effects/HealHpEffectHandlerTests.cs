using FastArena.Core.Domain.Effects;
using FastArena.Core.Services.Effects;
using FastArena.Core.Tests.Support;

namespace FastArena.Core.Tests;

public class HealHpEffectHandlerTests
{
    [Fact]
    public void OnRoundStart_HealsHeroAndClampsToMaxHealth()
    {
        var handler = new HealHpEffectHandler();
        var hero = EffectTestData.CreateHero(maxHealth: 100);
        var monster = EffectTestData.CreateMonster();
        var state = EffectTestData.CreateState(heroHealth: 70, heroAbility: 7);
        var effect = EffectTestData.CreateActiveEffect(EffectType.HEAL_HP, remainingRounds: 2, magnitude: 50);

        handler.OnRoundStart(effect, state, hero, monster);

        Assert.Equal(100, state.HeroHealth);
        Assert.Equal(10, state.HeroAbility);
    }

    [Fact]
    public void Stack_AveragesMagnitudeAndDuration()
    {
        var handler = new HealHpEffectHandler();
        var existing = EffectTestData.CreateActiveEffect(EffectType.HEAL_HP, remainingRounds: 3, magnitude: 60);
        var newDefinition = EffectTestData.CreateEffectDefinition(EffectType.HEAL_HP, durationRounds: 5, magnitude: 20);

        var merged = handler.Stack(existing, newDefinition);

        Assert.Equal(40, merged.Magnitude);
        Assert.Equal(4, merged.RemainingRounds);
        Assert.Equal(2, merged.StackCount);
    }
}