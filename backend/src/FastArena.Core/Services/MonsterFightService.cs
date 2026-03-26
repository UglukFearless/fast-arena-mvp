
using FastArena.Core.Domain;
using FastArena.Core.Domain.Activities;
using FastArena.Core.Domain.Activities.Actions;
using FastArena.Core.Domain.Activities.Datas;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.MondterFights;
using FastArena.Core.Exceptions;
using FastArena.Core.Interfaces.App;

namespace FastArena.Core.Services;

public class MonsterFightService : IMonsterFightService
{
    private readonly IUserService _userService;
    private readonly IActivityStateService _activityStateService;
    private readonly IHeroService _heroService;
    private readonly IMonsterFightResultService _monsterFightResultService;
    private readonly IHeroProgressService _heroProgressService;
    private readonly IActivityService _activityService;
    private readonly IActivitySessionService _activitySessionService;
    private readonly IItemService _itemService;

    private Dictionary<HitZone, int> _damageMap = new Dictionary<HitZone, int>
    {
        { HitZone.HEAD, 20 },
        { HitZone.LEFT_HAND, 4 },
        { HitZone.RIGHT_HAND, 4 },
        { HitZone.LEFT_LEG, 5 },
        { HitZone.RIGHT_LEG, 5 },
        { HitZone.NECK, 25 },
        { HitZone.LEFT_CHEST, 15 },
        { HitZone.RIGHT_CHEST, 15 },
        { HitZone.STOMACH, 10 },
        { HitZone.GROIN, 10 },
        { HitZone.HEART, 30 },
    };

    private Dictionary<HitZone, string> _hitZoneNameMap = new Dictionary<HitZone, string>
    {
        { HitZone.HEAD, "голову" },
        { HitZone.LEFT_HAND, "левую руку" },
        { HitZone.RIGHT_HAND, "правую руку" },
        { HitZone.LEFT_LEG, "левую ногу" },
        { HitZone.RIGHT_LEG, "правую ногу" },
        { HitZone.NECK, "горло" },
        { HitZone.LEFT_CHEST, "левоё плечо" },
        { HitZone.RIGHT_CHEST, "правое плечо" },
        { HitZone.STOMACH, "живот" },
        { HitZone.GROIN, "пах" },
        { HitZone.HEART, "сердце" },
    };

    private Dictionary<ActivityAwardLevel, int> _rewardBaseCoinMap = new Dictionary<ActivityAwardLevel, int>
    {
        { ActivityAwardLevel.LOW, 10 },
        { ActivityAwardLevel.MEDIUM, 20 },
        { ActivityAwardLevel.HIGH, 35 },
    };

    public MonsterFightService(
        IUserService userService, 
        IActivityStateService activityStateService, 
        IHeroService heroService,
        IMonsterFightResultService monsterFightResultService,
        IHeroProgressService heroProgressService,
        IActivityService activityService,
        IItemService itemService,
        IActivitySessionService activitySessionService
        )
    {
        _userService = userService;
        _activityStateService = activityStateService;
        _heroService = heroService;
        _monsterFightResultService = monsterFightResultService;
        _heroProgressService = heroProgressService;
        _activityService = activityService;
        _itemService = itemService;
        _activitySessionService = activitySessionService;
    }

    public async Task<MonsterFightRoundResult> CalcRoundAsync(HeroActVariant heroActVariant, Guid userId)
    {
        var user = await _userService.GetAsync(userId);
        ValidateUser(userId, user);
        var activityState = await _activityStateService.GetByHeroIdAsync(user.SelectedHeroId.Value);
        ValidateActivityState(activityState);

        return await ActRound(heroActVariant, activityState!);
    }

    private async Task<MonsterFightRoundResult> ActRound(
        HeroActVariant heroActVariant, 
        ActivitySession activityState
        )
    {
        var sessionLock = await _activityStateService.GetLockerForSessionIdAsync(activityState.Id);
        if (!await sessionLock.WaitAsync(0))
            throw new ActionDeniedException($"The activity with id {activityState.Id} is already acting rifht now!");

        var cleanTheLocker = false;
        var activityStateId = activityState.Id;

        try
        {
            var monsterFight = await BuildMonsterFight(activityState);
            var lastState = GetTheLastState(monsterFight);
            ValidateActVariant(heroActVariant, lastState.Value);
            
            switch (heroActVariant)
            {
                case HeroActVariant.ATTACK:
                    await DoAttack(monsterFight, activityState);
                    break;
                case HeroActVariant.FINALIZE:
                    await DoFinalize(monsterFight, activityState.Id);
                    cleanTheLocker = true;
                    return new MonsterFightRoundResult
                    {
                        ShoudGoNext = true,
                    };
            }

            var newMonsterFight = await BuildMonsterFight(activityState);
            var roundResult = BuildMonsterFightRoundResult(newMonsterFight);
            return roundResult;
        }
        finally
        {
            sessionLock.Release();

            if ( cleanTheLocker )
            {
                await _activityStateService.CleanTheLocker(activityStateId);
                sessionLock.Dispose();
            }
        }
    }

    private MonsterFightRoundResult BuildMonsterFightRoundResult(MonsterFight fight)
    {
        var newState = GetTheLastState(fight);
        return new MonsterFightRoundResult
        {
            ShoudGoNext = false,
            StateOrder = newState.Key,
            State = newState.Value,
            Reward = fight.Reward,
        };
    }

    private async Task DoAttack(MonsterFight monsterFight, ActivitySession activityState)
    {
        var lastState = GetTheLastState(monsterFight);
        var lastStateValue = lastState.Value;
        var rnd = new Random();
        var heroDiceRoll = rnd.Next(1, 7);
        var monsterDiceRoll = rnd.Next(1, 7);

        var diceRollBalance = heroDiceRoll - monsterDiceRoll;
        var abilityBalance = (int)((lastStateValue.HeroAbility - lastStateValue.MonsterAbility) / 3);
        abilityBalance = Math.Abs(abilityBalance) > 3 ? (abilityBalance / Math.Abs(abilityBalance)) * 3 : abilityBalance;

        var roundResultType = MonsterFightActionStateResultType.DRAW;
        var strikeStrength = 0;

        if (diceRollBalance < 0 && (diceRollBalance + abilityBalance) < 0)
        {
            roundResultType = MonsterFightActionStateResultType.STRIKE_BY_MONSTER;
            strikeStrength = diceRollBalance;
            if (abilityBalance > 0)
            {
                strikeStrength += abilityBalance;
            }
            strikeStrength = Math.Abs(strikeStrength);
        }

        if (diceRollBalance > 0 && (diceRollBalance + abilityBalance) > 0)
        {
            roundResultType = MonsterFightActionStateResultType.STRIKE_BY_HERO;
            strikeStrength = diceRollBalance;
            if (abilityBalance < 0)
            {
                strikeStrength += abilityBalance;
            }
            strikeStrength = Math.Abs(strikeStrength);
        }

        var hitZone = HitZone.HEAD;
        if (strikeStrength > 0)
        {
            var zoneDiceRoll = rnd.Next(1, 7);
            if (zoneDiceRoll != 6)
            {
                switch (zoneDiceRoll)
                {
                    case 1:
                        hitZone = HitZone.HEAD;
                        break;
                    case 2:
                        hitZone = HitZone.LEFT_HAND;
                        break;
                    case 3:
                        hitZone = HitZone.RIGHT_HAND;
                        break;
                    case 4:
                        hitZone = HitZone.LEFT_LEG;
                        break;
                    case 5:
                        hitZone = HitZone.RIGHT_LEG;
                        break;
                }
            } 
            else
            {
                zoneDiceRoll = rnd.Next(1, 7);
                switch (zoneDiceRoll)
                {
                    case 1:
                        hitZone = HitZone.GROIN;
                        break;
                    case 2:
                        hitZone = HitZone.STOMACH;
                        break;
                    case 3:
                        hitZone = HitZone.RIGHT_CHEST;
                        break;
                    case 4:
                        hitZone = HitZone.LEFT_CHEST;
                        break;
                    case 5:
                        hitZone = HitZone.NECK;
                        break;
                    case 6:
                        hitZone = HitZone.HEART;
                        break;
                }
            }

        }

        var newHeroHealth = lastState.Value.HeroHealth;
        var newMonsterHealth = lastState.Value.MonsterHealth;

        if (roundResultType == MonsterFightActionStateResultType.STRIKE_BY_MONSTER)
        {
            newHeroHealth = GetNewHeroHealth(lastState, strikeStrength, hitZone);
        } 
        else
        {
            newMonsterHealth = GetNewMonsterHealth(lastState, strikeStrength, hitZone);
        }

        var newState = await BuildNewMonsterFightState(
                monsterFight,
                lastState,
                roundResultType,
                heroDiceRoll,
                newHeroHealth,
                monsterDiceRoll,
                newMonsterHealth,
                strikeStrength,
                hitZone
            );

        activityState.State.Add(newState.Key, newState.Value);

        if (roundResultType == MonsterFightActionStateResultType.STRIKE_BY_HERO && newMonsterHealth <= 0)
        {
            ((MonsterFightData)activityState.CurrentAction.Data).Reward = 
                await GenerateReward(
                        monsterFight,
                        activityState
                    );
        }

        await _activityStateService.UpdateStateAsync( activityState );
    }

    private async Task<MonsterFightReward> GenerateReward(MonsterFight monsterFight, ActivitySession activityState)
    {
        var rnd = new Random();
        var activity = await _activityService.GetAsync(activityState.ActivityId);
        var hero = monsterFight.Hero;
        var monster = monsterFight.Monster;
        var baseCoinAmound = _rewardBaseCoinMap[activity.AwardLevel];

        var coinReward = baseCoinAmound
            + (int) (rnd.Next(5) * ((double) monster.Level / hero.Level))
            + (int) (rnd.Next(10) * ((double) monster.MaxHealth / hero.MaxHealth));

        var coinItem = await _itemService.GetBaseMoneyItemAsync();

        return new MonsterFightReward
        {
            Items = new List<GivenItem>
            {
                new GivenItem
                {
                    Item = coinItem,
                    Amount = coinReward,
                }
            }
        };
    }

    private async Task<KeyValuePair<int, MonsterFightActionState>> BuildNewMonsterFightState(
        MonsterFight monsterFight,
        KeyValuePair<int, MonsterFightActionState> lastState,
        MonsterFightActionStateResultType roundResultType,
        int heroDiceRoll, 
        int newHeroHealth, 
        int monsterDiceRoll, 
        int newMonsterHealth, 
        int strikeStrength, 
        HitZone hitZone)
    {
        var newOrder = lastState.Key + 1;

        var newHeroAbility = newHeroHealth / 10;
        var newMonsterAbility = newMonsterHealth / 10;

        var damage = 0;
        if (roundResultType == MonsterFightActionStateResultType.STRIKE_BY_MONSTER)
            damage = lastState.Value.HeroHealth - newHeroHealth;
        if (roundResultType == MonsterFightActionStateResultType.STRIKE_BY_HERO)
            damage = lastState.Value.MonsterHealth - newMonsterHealth;

        var newState = new MonsterFightActionState
        {
            HeroHealth = newHeroHealth,
            HeroAbility = newHeroAbility,
            HeroDiceRoll = heroDiceRoll,
            MonsterHealth = newMonsterHealth,
            MonsterAbility = newMonsterAbility,
            MonsterDiceRoll = monsterDiceRoll,
            ActVariants = (newHeroHealth <= 0 || newMonsterHealth <= 0) ?
                new HashSet<HeroActVariant> { HeroActVariant.FINALIZE } :
                new HashSet<HeroActVariant> { HeroActVariant.ATTACK },
            Result = await BuildMonsterFightActionStateResult(
                monsterFight, 
                strikeStrength, 
                hitZone, 
                roundResultType,
                newHeroHealth,
                newMonsterHealth,
                damage),
        };

        return new KeyValuePair<int, MonsterFightActionState>(newOrder, newState);
    }

    private async Task<MonsterFightActionStateResult> BuildMonsterFightActionStateResult(
        MonsterFight monsterFight, 
        int strikeStrength, 
        HitZone hitZone, 
        MonsterFightActionStateResultType roundResultType,
        int newHeroHealth,
        int newMonsterHealth,
        int damage)
    {
        if (roundResultType == MonsterFightActionStateResultType.DRAW)
        {
            return await Task.FromResult(new MonsterFightActionStateResult
            {
                ResultType = roundResultType,
                ResultText = "Ничья!",
            });
        }

        var monster = monsterFight.Monster;
        var hero = monsterFight.Hero;

        if (roundResultType == MonsterFightActionStateResultType.STRIKE_BY_MONSTER)
        {
            var newStateResult = new MonsterFightActionStateResult
            {
                ResultType = roundResultType,
                ResultText = $"{monster.Name} нанёс удар {hero.Name} " +
                    $" в {_hitZoneNameMap[hitZone]} на {damage} hp с силой {strikeStrength}.",
            };

            if (newHeroHealth <= 0)
            {
                newStateResult.ResultText +=  $" {monster.Name} убивает {hero.Name}!";
            }

            return await Task.FromResult(newStateResult);
        } 
        else
        {
            var newStateResult = new MonsterFightActionStateResult
            {
                ResultType = roundResultType,
                ResultText = $"{hero.Name} нанёс удар {monster.Name} " +
                    $" в {_hitZoneNameMap[hitZone]} на {damage} hp с силой {strikeStrength}.",
            };

            if (newMonsterHealth <= 0)
            {
                newStateResult.ResultText += $" {hero.Name} убивает {monster.Name}!";
            }

            return await Task.FromResult(newStateResult);
        }
    }

    private int GetNewHeroHealth(
        KeyValuePair<int, MonsterFightActionState>  lastState, 
        int strikeStrength, 
        HitZone hitZone)
    {
        var damage = strikeStrength * _damageMap[hitZone];
        return lastState.Value.HeroHealth - damage;
    }

    private int GetNewMonsterHealth(
        KeyValuePair<int, MonsterFightActionState> lastState,
        int strikeStrength,
        HitZone hitZone)
    {
        var damage = strikeStrength * _damageMap[hitZone];
        return lastState.Value.MonsterHealth - damage;
    }

    private async Task DoFinalize(MonsterFight fight, Guid activityId)
    {
        var lastState = GetTheLastState(fight);
        if (
            lastState.Value.HeroHealth > 0 && 
            lastState.Value.MonsterHealth <= 0 && 
            lastState.Value.Result?.ResultType == MonsterFightActionStateResultType.STRIKE_BY_HERO)
        {
            await _monsterFightResultService.AddWinResultAsync(fight);
            if (fight.Reward != null)
            {
                await _heroService.GiveItemsToHeroAsync(fight.Hero.Id, fight.Reward.Items);
            }
            var exp = await _heroProgressService.CalcExperienceForFightAsync(fight);
            await _heroService.IncreaseExperienceAsync(exp, fight.Hero.Id);
        } 
        else
        {
            await _monsterFightResultService.AddLoseResultAsync(fight);
            await _heroService.KillTheHeroAsync(fight.Hero.Id);
        }

        var activitySession = await _activityStateService.GetAsync(activityId);

        if (await _activitySessionService.HasNextAsync(activitySession))
        {
            await _activitySessionService.GoNextAsync(activitySession);
        } 
        else
        {
            await _activityStateService.CleanTheStateAsync(activityId);
        }
    }

    private void ValidateActVariant(HeroActVariant heroActVariant, MonsterFightActionState lastState)
    {
        if (!lastState.ActVariants.Any(av => av == heroActVariant))
            throw new ActionDeniedException($"The hero can't act {heroActVariant.ToString()}");
    }

    private KeyValuePair<int, MonsterFightActionState> GetTheLastState(MonsterFight fight)
    {
        return fight.State.MaxBy(s => s.Key);
    }

    public async Task<MonsterFight> GetByUserIdAsync(Guid userId)
    {
        var user = await _userService.GetAsync(userId);
        ValidateUser(userId, user);
        var activityState = await _activityStateService.GetByHeroIdAsync(user.SelectedHeroId!.Value);
        ValidateActivityState(activityState);

        return await BuildMonsterFight(activityState!);
    }

    private async Task<MonsterFight> BuildMonsterFight(ActivitySession activityState)
    {
        var hero = await _heroService.GetAsync(activityState.HeroId);
        var data = (MonsterFightData) activityState.CurrentAction.Data;
        var state = activityState.State.Select(s =>
        {
            return new KeyValuePair<int, MonsterFightActionState>(
                s.Key,
                (MonsterFightActionState) s.Value
                );
        });

        return new MonsterFight
        {
            Hero = hero,
            Monster = data.Monster,
            State = state.ToDictionary(),
            Reward = data.Reward,
        };
    }

    private void ValidateActivityState(ActivitySession? activityState)
    {
        if (activityState == null)
        {
            throw (new Exception("There is no any activity sessions for selected user hero."));
        }

        if (activityState.CurrentAction.Type != ActivityActionType.MONSTER_FIGHT)
        {
            throw new Exception($"The current Action has wrong type - {activityState.CurrentAction.Type}");
        }
    }

    private void ValidateUser(Guid userId, User user)
    {
        if (user == null)
        {
            throw new Exception($"An user with id {userId} doesn't exist.");
        }

        if (!user.SelectedHeroId.HasValue)
        {
            throw new Exception($"The user with id {userId} doesn't have a selected hero.");
        }
    }
}