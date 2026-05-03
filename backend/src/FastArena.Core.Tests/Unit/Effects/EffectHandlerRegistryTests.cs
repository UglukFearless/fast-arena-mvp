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
        var unitDamage = new UnitDamageDeltaEffectHandler();
        var incomingBlock = new IncomingStrikeFullBlockEffectHandler();
        var registry = new EffectHandlerRegistry(new IEffectHandler[] { heal, ability, strike, unitDamage, incomingBlock });

        Assert.Same(heal, registry.GetHandler(EffectType.HEAL_HP));
        Assert.Same(ability, registry.GetHandler(EffectType.OVERRIDE_ABILITY_TO_MAX));
        Assert.Same(strike, registry.GetHandler(EffectType.STRIKE_POWER_BONUS));
        Assert.Same(unitDamage, registry.GetHandler(EffectType.UNIT_DAMAGE_DELTA));
        Assert.Same(incomingBlock, registry.GetHandler(EffectType.INCOMING_STRIKE_FULL_BLOCK));
    }

    [Fact]
    public void GetHandler_ThrowsForUnregisteredType()
    {
        var registry = new EffectHandlerRegistry(new[] { new HealHpEffectHandler() });

        Assert.Throws<InvalidOperationException>(() => registry.GetHandler(EffectType.STRIKE_POWER_BONUS));
    }
}