using FastArena.Core.Domain.Activities.Actions;
using FastArena.Core.Domain.Effects;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Monsters;

namespace FastArena.Core.Tests.Support;

internal static class EffectTestData
{
    public static Hero CreateHero(int maxHealth = 100)
    {
        return new Hero
        {
            Id = Guid.NewGuid(),
            Name = "Hero",
            Sex = HeroSex.NONE,
            Level = 1,
            Experience = 0,
            MaxHealth = maxHealth,
            UserId = Guid.NewGuid(),
        };
    }

    public static Monster CreateMonster(int maxHealth = 100)
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

    public static MonsterFightActionState CreateState(
        int heroHealth = 60,
        int heroAbility = 6,
        int monsterHealth = 80,
        int monsterAbility = 8,
        int strikeStrength = 0)
    {
        return new MonsterFightActionState
        {
            HeroHealth = heroHealth,
            HeroAbility = heroAbility,
            MonsterHealth = monsterHealth,
            MonsterAbility = monsterAbility,
            StrikeStrength = strikeStrength,
            ActVariants = new HashSet<HeroActVariant> { HeroActVariant.ATTACK },
        };
    }

    public static ActiveEffect CreateActiveEffect(
        EffectType type,
        int remainingRounds,
        int magnitude,
        int stackCount = 1)
    {
        return new ActiveEffect
        {
            DefinitionId = Guid.NewGuid(),
            Type = type,
            RemainingRounds = remainingRounds,
            Magnitude = magnitude,
            MinValue = 0,
            MaxValue = 0,
            ChancePercent = 100,
            ConditionType = EffectConditionType.ALWAYS,
            TargetType = type == EffectType.STRIKE_POWER_BONUS ? EffectTargetType.CONTEXT_VALUE : EffectTargetType.SELF,
            Priority = 0,
            StackCount = stackCount,
        };
    }

    public static EffectDefinition CreateEffectDefinition(EffectType type, int durationRounds, int magnitude)
    {
        return new EffectDefinition
        {
            Id = Guid.NewGuid(),
            ItemId = Guid.NewGuid(),
            Type = type,
            DurationRounds = durationRounds,
            Magnitude = magnitude,
            MinValue = 0,
            MaxValue = 0,
            ChancePercent = 100,
            ConditionType = EffectConditionType.ALWAYS,
            TargetType = type == EffectType.STRIKE_POWER_BONUS ? EffectTargetType.CONTEXT_VALUE : EffectTargetType.SELF,
            Priority = 0,
        };
    }
}