using FastArena.Core.Domain;
using FastArena.Core.Domain.Activities.Actions;
using FastArena.Core.Domain.Activities.Datas;
using FastArena.Core.Domain.Effects;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Monsters;

namespace FastArena.Core.Tests;

public class MonsterFightDataTests
{
    [Fact]
    public async Task BuildInitStateAsync_WithoutPreviousState_StartsFromHeroMaxHealth()
    {
        var hero = CreateHero(maxHealth: 100);
        var data = new MonsterFightData { Monster = CreateMonster(maxHealth: 80) };

        var state = (MonsterFightActionState)await data.BuildInitStateAsync(
            hero,
            CreateCase(data),
            previousState: null);

        Assert.Equal(100, state.HeroHealth);
        Assert.Equal(10, state.HeroAbility);
        Assert.Equal(80, state.MonsterHealth);
        Assert.Empty(state.ActiveEffects);
    }

    [Fact]
    public async Task BuildInitStateAsync_WithPreviousState_CarriesHealthAndEquipmentEffects()
    {
        var hero = CreateHero(maxHealth: 100);
        var data = new MonsterFightData { Monster = CreateMonster(maxHealth: 80) };
        var previousState = new MonsterFightActionState
        {
            HeroHealth = 40,
            ActVariants = new HashSet<HeroActVariant>(),
            ActiveEffects = new List<ActiveEffect>
            {
                CreateEffect(EffectSourceType.Equipment, EffectType.INCOMING_STRIKE_FULL_BLOCK, EffectLifetimeType.Persistent, magnitude: 1),
                CreateEffect(EffectSourceType.Potion, EffectType.HEAL_HP, EffectLifetimeType.RoundBased, magnitude: 60),
            },
        };

        var state = (MonsterFightActionState)await data.BuildInitStateAsync(
            hero,
            CreateCase(data),
            previousState);

        Assert.Equal(40, state.HeroHealth);
        Assert.Equal(4, state.HeroAbility);

        var carriedEffect = Assert.Single(state.ActiveEffects);
        Assert.Equal(EffectSourceType.Equipment, carriedEffect.SourceType);
        Assert.Equal(1, carriedEffect.Magnitude);
    }

    private static ActivityActionCase CreateCase(MonsterFightData data)
    {
        return new ActivityActionCase
        {
            Type = ActivityActionType.MONSTER_FIGHT,
            Data = data,
        };
    }

    private static Hero CreateHero(int maxHealth)
    {
        return new Hero
        {
            Id = Guid.NewGuid(),
            Name = "Hero",
            Sex = HeroSex.NONE,
            Level = 1,
            Experience = 0,
            MaxHealth = maxHealth,
            Portrait = new Portrait { Id = Guid.NewGuid(), Url = "/portraits/hero.png" },
        };
    }

    private static Monster CreateMonster(int maxHealth)
    {
        return new Monster
        {
            Id = Guid.NewGuid(),
            Name = "Monster",
            Level = 1,
            MaxHealth = maxHealth,
            MonsterMoldId = Guid.NewGuid(),
            Mold = null!,
            Portrait = null!,
        };
    }

    private static ActiveEffect CreateEffect(
        EffectSourceType sourceType,
        EffectType type,
        EffectLifetimeType lifetimeType,
        int magnitude)
    {
        return new ActiveEffect
        {
            DefinitionId = Guid.NewGuid(),
            Type = type,
            SourceType = sourceType,
            LifetimeType = lifetimeType,
            Magnitude = magnitude,
            RemainingRounds = lifetimeType == EffectLifetimeType.RoundBased ? 3 : 0,
            ChancePercent = 100,
            TargetType = EffectTargetType.SELF,
            StackCount = 1,
        };
    }
}
