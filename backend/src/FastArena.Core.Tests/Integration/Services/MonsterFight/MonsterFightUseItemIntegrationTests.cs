using FastArena.Core.Domain;
using FastArena.Core.Domain.Activities;
using FastArena.Core.Domain.Activities.Actions;
using FastArena.Core.Domain.Activities.Datas;
using FastArena.Core.Domain.Effects;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Items;
using FastArena.Core.Domain.MondterFights;
using FastArena.Core.Domain.Monsters;
using FastArena.Core.Interfaces.App;
using FastArena.Core.Interfaces.Effects;
using FastArena.Core.Models;
using FastArena.Core.Services;
using FastArena.Core.Services.Effects;

namespace FastArena.Core.Tests;

public class MonsterFightUseItemIntegrationTests
{
    [Fact]
    public async Task CalcRoundAsync_UseItem_UpdatesFightStateAndConsumesPocketItem()
    {
        var userId = Guid.NewGuid();
        var heroId = Guid.NewGuid();
        var heroItemCellId = Guid.NewGuid();

        var healDefinition = new EffectDefinition
        {
            Id = Guid.NewGuid(),
            ItemId = Guid.NewGuid(),
            Type = EffectType.HEAL_HP,
            DurationRounds = 1,
            Magnitude = 30,
            MinValue = 0,
            MaxValue = 0,
            ChancePercent = 100,
            ConditionType = EffectConditionType.ALWAYS,
            TargetType = EffectTargetType.SELF,
            Priority = 1,
        };

        var potion = new Item
        {
            Id = healDefinition.ItemId,
            Name = "Test Potion",
            Description = "Integration test item",
            BaseCost = 1,
            ItemImage = "potion.png",
            CanBeEquipped = true,
            CanBeFolded = true,
            Type = ItemType.POTION,
            Effects = new List<EffectDefinition> { healDefinition },
        };

        var heroItemCell = new HeroItemCell
        {
            Id = heroItemCellId,
            HeroId = heroId,
            ItemId = potion.Id,
            Amount = 1,
            Item = potion,
        };

        var hero = new Hero
        {
            Id = heroId,
            Name = "Integration Hero",
            Sex = HeroSex.NONE,
            Level = 1,
            Experience = 0,
            MaxHealth = 100,
            UserId = userId,
            EquippedSlots = new List<HeroEquippedSlot>
            {
                new HeroEquippedSlot
                {
                    HeroId = heroId,
                    Slot = EquipmentSlotType.POCKET_1,
                    HeroItemCellId = heroItemCellId,
                    HeroItemCell = heroItemCell,
                },
            },
        };

        var user = new User
        {
            Id = userId,
            Login = "integration-user",
            PasswordHash = "hash",
            SelectedHeroId = heroId,
        };

        var monster = new Monster
        {
            Id = Guid.NewGuid(),
            Name = "Training Monster",
            Level = 1,
            MaxHealth = 100,
            MonsterMoldId = Guid.NewGuid(),
            Mold = null!,
            Portrait = null!,
        };

        var action = new ActivityActionCase
        {
            Type = ActivityActionType.MONSTER_FIGHT,
            Data = new MonsterFightData
            {
                Monster = monster,
            },
        };

        var initState = (MonsterFightActionState)await action.BuildInitStateAsync(hero, action);

        var session = new ActivitySession
        {
            Id = Guid.NewGuid(),
            HeroId = heroId,
            ActivityId = Guid.NewGuid(),
            CurrentAction = action,
            Actions = new List<ActivityActionCase> { action },
        };
        session.State[0] = initState;

        var userService = new FakeUserService(user);
        var heroService = new FakeHeroService(hero, heroItemCell);
        var activityStateService = new FakeActivityStateService(session);

        var monsterFightService = new MonsterFightService(
            userService,
            activityStateService,
            heroService,
            new FakeMonsterFightResultService(),
            new FakeHeroProgressService(),
            new FakeActivityService(session.ActivityId),
            new FakeItemService(),
            new FakeActivitySessionService(),
            new EffectHandlerRegistry(new IEffectHandler[]
            {
                new HealHpEffectHandler(),
                new OverrideAbilityToMaxEffectHandler(),
                new StrikePowerBonusEffectHandler(),
            }));

        var roundResult = await monsterFightService.CalcRoundAsync(
            new MonsterFightActionPayload
            {
                ActVariant = HeroActVariant.USE_ITEM,
                ActionData = new MonsterFightActionData
                {
                    UsedPocketItemCellId = heroItemCellId,
                },
            },
            userId);

        Assert.False(roundResult.ShoudGoNext);
        Assert.NotNull(roundResult.State);
        Assert.NotNull(roundResult.StateOrder);
        Assert.Equal(1, roundResult.StateOrder);

        var currentState = roundResult.State!;
        Assert.DoesNotContain(currentState.PocketItems, p => p.Id == heroItemCellId);
        Assert.Contains(currentState.ActiveEffects, e => e.Type == EffectType.HEAL_HP);
        Assert.DoesNotContain(HeroActVariant.USE_ITEM, currentState.ActVariants);
        Assert.Contains(HeroActVariant.ATTACK, currentState.ActVariants);
    }

    private sealed class FakeUserService : IUserService
    {
        private readonly User _user;

        public FakeUserService(User user)
        {
            _user = user;
        }

        public Task<User> CreateAsync(string login, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByLoginAsync(string login)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetAsync(Guid id)
        {
            return Task.FromResult(_user);
        }
    }

    private sealed class FakeHeroService : IHeroService
    {
        private readonly Hero _hero;
        private readonly HeroItemCell _consumable;

        public FakeHeroService(Hero hero, HeroItemCell consumable)
        {
            _hero = hero;
            _consumable = consumable;
        }

        public Task<ICollection<Hero>> GetAllByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Hero>> GetAllWithInfoByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Hero>> GetAllWithInfoAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Hero> GetSelectedByUserIdAsync(Guid userId)
        {
            return Task.FromResult(_hero);
        }

        public Task<Hero> CreateAsync(HeroCreationModel model)
        {
            throw new NotImplementedException();
        }

        public Task SelectForUserAsync(Guid id, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task UnselectForUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task UnselectForUserForceAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<Hero> GetAsync(Guid id)
        {
            return Task.FromResult(_hero);
        }

        public Task KillTheHeroAsync(Guid id)
        {
            return Task.CompletedTask;
        }

        public Task GiveItemsToHeroAsync(Guid heroId, ICollection<GivenItem> items)
        {
            return Task.CompletedTask;
        }

        public Task<HeroItemCell> ConsumePocketItemForFightAsync(Guid heroId, Guid heroItemCellId)
        {
            if (_hero.EquippedSlots == null)
            {
                throw new InvalidOperationException("Hero has no slots.");
            }

            var slot = _hero.EquippedSlots.FirstOrDefault(s => s.HeroItemCellId == heroItemCellId);
            if (slot?.HeroItemCell == null)
            {
                throw new InvalidOperationException("Pocket item not found.");
            }

            var snapshot = new HeroItemCell
            {
                Id = slot.HeroItemCell.Id,
                HeroId = slot.HeroItemCell.HeroId,
                ItemId = slot.HeroItemCell.ItemId,
                Amount = slot.HeroItemCell.Amount,
                Item = slot.HeroItemCell.Item,
            };

            slot.HeroItemCell.Amount -= 1;
            if (slot.HeroItemCell.Amount <= 0)
            {
                slot.HeroItemCell = null;
                slot.HeroItemCellId = null;
            }

            return Task.FromResult(snapshot);
        }

        public Task IncreaseExperienceAsync(int experience, Guid heroId)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class FakeActivityStateService : IActivityStateService
    {
        private ActivitySession _session;
        private readonly Dictionary<Guid, SemaphoreSlim> _locks = new();

        public FakeActivityStateService(ActivitySession session)
        {
            _session = session;
        }

        public Task<bool> IsBusyAsync(Guid userId)
        {
            return Task.FromResult(true);
        }

        public Task CreateStateAsync(ActivitySession session)
        {
            _session = session;
            return Task.CompletedTask;
        }

        public Task UpdateStateAsync(ActivitySession session)
        {
            _session = session;
            return Task.CompletedTask;
        }

        public Task<ActivitySession?> GetByHeroIdAsync(Guid heroId)
        {
            return Task.FromResult(_session.HeroId == heroId ? _session : null);
        }

        public Task<ActivitySession?> GetAsync(Guid id)
        {
            return Task.FromResult(_session.Id == id ? _session : null);
        }

        public Task<SemaphoreSlim> GetLockerForSessionIdAsync(Guid sessionId)
        {
            if (!_locks.TryGetValue(sessionId, out var locker))
            {
                locker = new SemaphoreSlim(1, 1);
                _locks[sessionId] = locker;
            }

            return Task.FromResult(locker);
        }

        public Task CleanTheStateAsync(Guid id)
        {
            return Task.CompletedTask;
        }

        public Task CleanTheLocker(Guid id)
        {
            _locks.Remove(id);
            return Task.CompletedTask;
        }
    }

    private sealed class FakeMonsterFightResultService : IMonsterFightResultService
    {
        public Task<MonsterFightResult> AddWinResultAsync(MonsterFight fight)
        {
            throw new NotImplementedException();
        }

        public Task<MonsterFightResult> AddLoseResultAsync(MonsterFight fight)
        {
            throw new NotImplementedException();
        }
    }

    private sealed class FakeHeroProgressService : IHeroProgressService
    {
        public Task<HeroLevelProgressInfo> GetInfoByLevelAsync(int level)
        {
            throw new NotImplementedException();
        }

        public Task<int> CalcExperienceForFightAsync(MonsterFight fight)
        {
            return Task.FromResult(0);
        }
    }

    private sealed class FakeActivityService : IActivityService
    {
        private readonly Guid _activityId;

        public FakeActivityService(Guid activityId)
        {
            _activityId = activityId;
        }

        public Task<ICollection<Activity>> GetForSelectedHeroByUserId(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Activity>> GetByHero(Hero hero)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsActivityAvailableForHero(Guid activityId, Hero hero)
        {
            return Task.FromResult(activityId == _activityId);
        }

        public Task<Activity> GetAsync(Guid id)
        {
            return Task.FromResult(new Activity
            {
                Id = id,
                Title = "Arena",
                ActivationTitle = "Fight",
                IconUrl = "arena.png",
                DangerLevel = ActivityDangerLevel.LOW,
                AwardLevel = ActivityAwardLevel.LOW,
                Type = ActivityType.REGULAR,
                Actions = new List<ActivityAction>(),
            });
        }
    }

    private sealed class FakeItemService : IItemService
    {
        public Task<Item> GetBaseMoneyItemAsync()
        {
            return Task.FromResult(new Item
            {
                Id = Guid.NewGuid(),
                Name = "Coin",
                Description = "Coin",
                BaseCost = 1,
                ItemImage = "coin.png",
                CanBeEquipped = false,
                CanBeFolded = true,
                Type = ItemType.MONEY,
            });
        }
    }

    private sealed class FakeActivitySessionService : IActivitySessionService
    {
        public Task<ActivitySession> StartActivityAsync(Guid activityId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<ActivitySession> GetCurrentActivityByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasNextAsync(ActivitySession activitySession)
        {
            return Task.FromResult(false);
        }

        public Task<ActivitySession> GoNextAsync(ActivitySession activitySession)
        {
            return Task.FromResult(activitySession);
        }
    }
}