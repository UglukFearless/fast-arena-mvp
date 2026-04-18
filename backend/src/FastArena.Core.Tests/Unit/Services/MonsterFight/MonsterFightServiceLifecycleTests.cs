using System.Reflection;
using FastArena.Core.Domain.Activities.Actions;
using FastArena.Core.Domain.Effects;
using FastArena.Core.Services;
using FastArena.Core.Tests.Support;

namespace FastArena.Core.Tests;

public class MonsterFightServiceLifecycleTests
{
    [Fact]
    public void DecrementEffectDurations_DoesNotRemoveExpiredEffectsByItself()
    {
        var state = new MonsterFightActionState
        {
            HeroHealth = 100,
            HeroAbility = 10,
            MonsterHealth = 100,
            MonsterAbility = 10,
            ActVariants = new HashSet<HeroActVariant> { HeroActVariant.ATTACK },
            ActiveEffects = new List<ActiveEffect>
            {
                EffectTestData.CreateActiveEffect(EffectType.HEAL_HP, remainingRounds: 1, magnitude: 30),
            },
        };

        var decrementMethod = typeof(MonsterFightService).GetMethod("DecrementEffectDurations", BindingFlags.NonPublic | BindingFlags.Static);

        Assert.NotNull(decrementMethod);

        decrementMethod!.Invoke(null, new object[] { state });

        Assert.Single(state.ActiveEffects);
        Assert.Equal(0, state.ActiveEffects[0].RemainingRounds);
    }

    [Fact]
    public void CleanupExpiredEffects_RemovesOnlyEffectsWithNonPositiveDuration()
    {
        var state = new MonsterFightActionState
        {
            HeroHealth = 100,
            HeroAbility = 10,
            MonsterHealth = 100,
            MonsterAbility = 10,
            ActVariants = new HashSet<HeroActVariant> { HeroActVariant.ATTACK },
            ActiveEffects = new List<ActiveEffect>
            {
                EffectTestData.CreateActiveEffect(EffectType.HEAL_HP, remainingRounds: 0, magnitude: 30),
                EffectTestData.CreateActiveEffect(EffectType.STRIKE_POWER_BONUS, remainingRounds: 2, magnitude: 2),
            },
        };

        var cleanupMethod = typeof(MonsterFightService).GetMethod("CleanupExpiredEffects", BindingFlags.NonPublic | BindingFlags.Static);

        Assert.NotNull(cleanupMethod);

        cleanupMethod!.Invoke(null, new object[] { state });

        Assert.Single(state.ActiveEffects);
        Assert.Equal(EffectType.STRIKE_POWER_BONUS, state.ActiveEffects[0].Type);
    }
}