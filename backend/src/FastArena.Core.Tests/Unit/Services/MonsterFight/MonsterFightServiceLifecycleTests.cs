using System.Reflection;
using FastArena.Core.Domain.Activities.Actions;
using FastArena.Core.Domain.Effects;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Items;
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
    public void DecrementEffectDurations_SkipsPersistentEffects()
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
                EffectTestData.CreateActiveEffect(EffectType.STRIKE_POWER_BONUS, remainingRounds: 7, magnitude: 1),
            },
        };
        state.ActiveEffects[0].LifetimeType = EffectLifetimeType.Persistent;
        state.ActiveEffects[0].SourceType = EffectSourceType.Equipment;

        var decrementMethod = typeof(MonsterFightService).GetMethod("DecrementEffectDurations", BindingFlags.NonPublic | BindingFlags.Static);

        Assert.NotNull(decrementMethod);

        decrementMethod!.Invoke(null, new object[] { state });

        Assert.Single(state.ActiveEffects);
        Assert.Equal(7, state.ActiveEffects[0].RemainingRounds);
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

    [Fact]
    public void CleanupExpiredEffects_KeepsPersistentEffects()
    {
        var persistent = EffectTestData.CreateActiveEffect(EffectType.INCOMING_STRIKE_FULL_BLOCK, remainingRounds: 0, magnitude: 0);
        persistent.LifetimeType = EffectLifetimeType.Persistent;
        persistent.SourceType = EffectSourceType.Equipment;

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
                persistent,
            },
        };

        var cleanupMethod = typeof(MonsterFightService).GetMethod("CleanupExpiredEffects", BindingFlags.NonPublic | BindingFlags.Static);

        Assert.NotNull(cleanupMethod);

        cleanupMethod!.Invoke(null, new object[] { state });

        Assert.Single(state.ActiveEffects);
        Assert.Equal(EffectType.INCOMING_STRIKE_FULL_BLOCK, state.ActiveEffects[0].Type);
    }

    [Fact]
    public void InitializeEquipmentEffects_AddsEquipmentEffectsFromHands()
    {
        var swordEffect = new EffectDefinition
        {
            Id = Guid.NewGuid(),
            ItemId = Guid.NewGuid(),
            Type = EffectType.STRIKE_POWER_BONUS,
            DurationRounds = 0,
            LifetimeType = EffectLifetimeType.Persistent,
            SourceType = EffectSourceType.Equipment,
            Magnitude = 1,
            MinValue = 1,
            MaxValue = 1,
            ChancePercent = 100,
            TargetType = EffectTargetType.CONTEXT_VALUE,
        };

        var sword = new Item
        {
            Id = swordEffect.ItemId,
            Name = "Test Sword",
            Description = "",
            BaseCost = 1,
            ItemImage = "sword.png",
            CanBeEquipped = true,
            CanBeFolded = false,
            Type = ItemType.WEAPON,
            Effects = new List<EffectDefinition> { swordEffect },
        };

        var cell = new HeroItemCell
        {
            Id = Guid.NewGuid(),
            HeroId = Guid.NewGuid(),
            ItemId = sword.Id,
            Amount = 1,
            Item = sword,
        };

        var hero = EffectTestData.CreateHero();
        hero.EquippedSlots = new List<HeroEquippedSlot>
        {
            new HeroEquippedSlot
            {
                HeroId = hero.Id,
                Slot = EquipmentSlotType.RIGHT_HAND,
                HeroItemCellId = cell.Id,
                HeroItemCell = cell,
            },
        };

        var state = new MonsterFightActionState
        {
            HeroHealth = 100,
            HeroAbility = 10,
            MonsterHealth = 100,
            MonsterAbility = 10,
            ActVariants = new HashSet<HeroActVariant> { HeroActVariant.ATTACK },
            ActiveEffects = new List<ActiveEffect>(),
        };

        var initMethod = typeof(MonsterFightService).GetMethod("InitializeEquipmentEffects", BindingFlags.NonPublic | BindingFlags.Static);

        Assert.NotNull(initMethod);

        initMethod!.Invoke(null, new object[] { state, hero });

        Assert.Single(state.ActiveEffects);
        var effect = state.ActiveEffects[0];
        Assert.Equal(EffectType.STRIKE_POWER_BONUS, effect.Type);
        Assert.Equal(EffectLifetimeType.Persistent, effect.LifetimeType);
        Assert.Equal(EffectSourceType.Equipment, effect.SourceType);
    }

    [Fact]
    public void InitializeEquipmentEffects_DoesNotDuplicateWhenEquipmentEffectsAlreadyPresent()
    {
        var hero = EffectTestData.CreateHero();
        hero.EquippedSlots = new List<HeroEquippedSlot>();

        var existing = EffectTestData.CreateActiveEffect(EffectType.STRIKE_POWER_BONUS, remainingRounds: 0, magnitude: 1);
        existing.LifetimeType = EffectLifetimeType.Persistent;
        existing.SourceType = EffectSourceType.Equipment;

        var state = new MonsterFightActionState
        {
            HeroHealth = 100,
            HeroAbility = 10,
            MonsterHealth = 100,
            MonsterAbility = 10,
            ActVariants = new HashSet<HeroActVariant> { HeroActVariant.ATTACK },
            ActiveEffects = new List<ActiveEffect> { existing },
        };

        var initMethod = typeof(MonsterFightService).GetMethod("InitializeEquipmentEffects", BindingFlags.NonPublic | BindingFlags.Static);

        Assert.NotNull(initMethod);

        initMethod!.Invoke(null, new object[] { state, hero });

        Assert.Single(state.ActiveEffects);
    }
}