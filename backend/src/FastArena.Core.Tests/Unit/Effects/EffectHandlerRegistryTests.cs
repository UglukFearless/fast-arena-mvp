using FastArena.Core.Domain.Effects;
using FastArena.Core.Interfaces.Effects;
using FastArena.Core.Services.Effects;

namespace FastArena.Core.Tests;

public class EffectHandlerRegistryTests
{
    [Fact]
    public void Constructor_MapsHandlersByEffectType()
    {
        var heal = new HealHpEffectHandler();
        var ability = new OverrideAbilityToMaxEffectHandler();
        var strike = new StrikePowerBonusEffectHandler();
        var registry = new EffectHandlerRegistry(new IEffectHandler[] { heal, ability, strike });

        Assert.Same(heal, registry.GetHandler(EffectType.HEAL_HP));
        Assert.Same(ability, registry.GetHandler(EffectType.OVERRIDE_ABILITY_TO_MAX));
        Assert.Same(strike, registry.GetHandler(EffectType.STRIKE_POWER_BONUS));
    }

    [Fact]
    public void GetHandler_ThrowsForUnregisteredType()
    {
        var registry = new EffectHandlerRegistry(new[] { new HealHpEffectHandler() });

        Assert.Throws<InvalidOperationException>(() => registry.GetHandler(EffectType.STRIKE_POWER_BONUS));
    }
}