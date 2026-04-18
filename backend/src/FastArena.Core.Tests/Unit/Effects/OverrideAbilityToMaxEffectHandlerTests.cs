using FastArena.Core.Domain.Effects;
using FastArena.Core.Services.Effects;
using FastArena.Core.Tests.Support;

namespace FastArena.Core.Tests;

public class OverrideAbilityToMaxEffectHandlerTests
{
    [Fact]
    public void OnStrikeClaimed_OverridesHeroAbilityToMaxHealthRatio()
    {
        var handler = new OverrideAbilityToMaxEffectHandler();
        var hero = EffectTestData.CreateHero(maxHealth: 140);
        var monster = EffectTestData.CreateMonster();
        var state = EffectTestData.CreateState(heroHealth: 30, heroAbility: 3);
        var effect = EffectTestData.CreateActiveEffect(EffectType.OVERRIDE_ABILITY_TO_MAX, remainingRounds: 2, magnitude: 0);

        handler.OnStrikeClaimed(effect, state, hero, monster);

        Assert.Equal(14, state.HeroAbility);
    }

    [Fact]
    public void Stack_SumsRemainingDurations()
    {
        var handler = new OverrideAbilityToMaxEffectHandler();
        var existing = EffectTestData.CreateActiveEffect(EffectType.OVERRIDE_ABILITY_TO_MAX, remainingRounds: 3, magnitude: 0);
        var newDefinition = EffectTestData.CreateEffectDefinition(EffectType.OVERRIDE_ABILITY_TO_MAX, durationRounds: 4, magnitude: 0);

        var merged = handler.Stack(existing, newDefinition);

        Assert.Equal(7, merged.RemainingRounds);
        Assert.Equal(2, merged.StackCount);
    }
}