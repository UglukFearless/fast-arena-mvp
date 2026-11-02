using FastArena.Core.Domain;
using FastArena.Core.Domain.Activities;
using FastArena.Core.Domain.Activities.Actions;
using FastArena.Core.Domain.Activities.Datas;
using FastArena.Core.Domain.Effects;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Monsters;
using FastArena.Core.Interfaces.App;
using FastArena.Core.Services;

namespace FastArena.Core.Tests;

public class ActivitySessionServiceTests
{
    [Fact]
    public async Task GoNextAsync_StartsNextEpisodeFromPreviousStateAndClearsRounds()
    {
        var hero = CreateHero(maxHealth: 100);
        var firstCase = CreateFightCase(maxHealth: 80);
        var secondCase = CreateFightCase(maxHealth: 60);
        var session = new ActivitySession
        {
            Id = Guid.NewGuid(),
            HeroId = hero.Id,
            ActivityId = Guid.NewGuid(),
            CurrentAction = firstCase,
            Actions = new List<ActivityActionCase> { firstCase, secondCase },
        };
        session.State[0] = CreateState(heroHealth: 100, monsterHealth: 80);
        session.State[3] = CreateState(
            heroHealth: 40,
            monsterHealth: 0,
            new List<ActiveEffect>
            {
                CreateEquipmentEffect(magnitude: 1),
                CreatePotionEffect(),
            });

        var service = new ActivitySessionService(
            new FakeHeroService(hero),
            new FakeActivityService(),
            new FakeActivityStateService(),
            new FakeMonsterService());

        await service.GoNextAsync(session);

        Assert.Same(secondCase, session.CurrentAction);
        Assert.Single(session.State);

        var state = Assert.IsType<MonsterFightActionState>(session.State[0]);
        Assert.Equal(40, state.HeroHealth);
        Assert.Equal(4, state.HeroAbility);
        Assert.Equal(60, state.MonsterHealth);

        var carriedEffect = Assert.Single(state.ActiveEffects);
        Assert.Equal(EffectSourceType.Equipment, carriedEffect.SourceType);
        Assert.Equal(1, carriedEffect.Magnitude);
    }

    private static ActivityActionCase CreateFightCase(int maxHealth)
    {
        return new ActivityActionCase
        {
            Type = ActivityActionType.MONSTER_FIGHT,
            Data = new MonsterFightData
            {
                Monster = new Monster
                {
                    Id = Guid.NewGuid(),
                    Name = "Monster",
                    Level = 1,
                    MaxHealth = maxHealth,
                    MonsterMoldId = Guid.NewGuid(),
                    Mold = null!,
                    Portrait = null!,
                },
            },
        };
    }

    private static MonsterFightActionState CreateState(
        int heroHealth,
        int monsterHealth,
        List<ActiveEffect>? activeEffects = null)
    {
        return new MonsterFightActionState
        {
            HeroHealth = heroHealth,
            MonsterHealth = monsterHealth,
            ActVariants = new HashSet<HeroActVariant>(),
            ActiveEffects = activeEffects ?? new List<ActiveEffect>(),
        };
    }

    private static ActiveEffect CreateEquipmentEffect(int magnitude)
    {
        return new ActiveEffect
        {
            DefinitionId = Guid.NewGuid(),
            Type = EffectType.INCOMING_STRIKE_FULL_BLOCK,
            SourceType = EffectSourceType.Equipment,
            LifetimeType = EffectLifetimeType.Persistent,
            Magnitude = magnitude,
            TargetType = EffectTargetType.SELF,
            ChancePercent = 50,
        };
    }

    private static ActiveEffect CreatePotionEffect()
    {
        return new ActiveEffect
        {
            DefinitionId = Guid.NewGuid(),
            Type = EffectType.HEAL_HP,
            SourceType = EffectSourceType.Potion,
            LifetimeType = EffectLifetimeType.RoundBased,
            Magnitude = 60,
            RemainingRounds = 2,
            TargetType = EffectTargetType.SELF,
            ChancePercent = 100,
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

    private sealed class FakeHeroService : IHeroService
    {
        private readonly Hero _hero;

        public FakeHeroService(Hero hero)
        {
            _hero = hero;
        }

        public Task<Hero> GetAsync(Guid id) => Task.FromResult(_hero);

        public Task<ICollection<Hero>> GetAllByUserIdAsync(Guid userId) => throw new NotImplementedException();
        public Task<ICollection<Hero>> GetAllWithInfoByUserIdAsync(Guid userId) => throw new NotImplementedException();
        public Task<ICollection<Hero>> GetAllWithInfoAsync() => throw new NotImplementedException();
        public Task<Hero> GetSelectedByUserIdAsync(Guid userId) => throw new NotImplementedException();
        public Task<Hero> CreateAsync(FastArena.Core.Models.HeroCreationModel model) => throw new NotImplementedException();
        public Task SelectForUserAsync(Guid id, Guid userId) => throw new NotImplementedException();
        public Task UnselectForUserAsync(Guid userId) => throw new NotImplementedException();
        public Task UnselectForUserForceAsync(Guid userId) => throw new NotImplementedException();
        public Task KillTheHeroAsync(Guid id) => throw new NotImplementedException();
        public Task GiveItemsToHeroAsync(Guid heroId, ICollection<GivenItem> items) => throw new NotImplementedException();
        public Task<HeroItemCell> ConsumePocketItemForFightAsync(Guid heroId, Guid heroItemCellId) => throw new NotImplementedException();
        public Task IncreaseExperienceAsync(int experience, Guid heroId) => throw new NotImplementedException();
    }

    private sealed class FakeActivityService : IActivityService
    {
        public Task<ICollection<Activity>> GetForSelectedHeroByUserId(Guid userId) => throw new NotImplementedException();
        public Task<ICollection<Activity>> GetByHero(Hero hero) => throw new NotImplementedException();
        public Task<bool> IsActivityAvailableForHero(Guid activityId, Hero hero) => throw new NotImplementedException();
        public Task<Activity> GetAsync(Guid id) => throw new NotImplementedException();
    }

    private sealed class FakeActivityStateService : IActivityStateService
    {
        public Task UpdateStateAsync(ActivitySession session) => Task.CompletedTask;

        public Task<bool> IsBusyAsync(Guid userId) => throw new NotImplementedException();
        public Task CreateStateAsync(ActivitySession session) => throw new NotImplementedException();
        public Task<ActivitySession?> GetByHeroIdAsync(Guid heroId) => throw new NotImplementedException();
        public Task<ActivitySession?> GetAsync(Guid id) => throw new NotImplementedException();
        public Task<SemaphoreSlim> GetLockerForSessionIdAsync(Guid sessionId) => throw new NotImplementedException();
        public Task CleanTheStateAsync(Guid id) => throw new NotImplementedException();
        public Task CleanTheLocker(Guid id) => throw new NotImplementedException();
    }

    private sealed class FakeMonsterService : IMonsterService
    {
        public Task<ICollection<MonsterMold>> GetAllMoldsAsync() => throw new NotImplementedException();
        public Task<MonsterMold> GetRandomMoldForRankAsync(int rank) => throw new NotImplementedException();
        public Task<Monster> CreateFromMoldAsync(int level, MonsterMold mold) => throw new NotImplementedException();
    }
}
