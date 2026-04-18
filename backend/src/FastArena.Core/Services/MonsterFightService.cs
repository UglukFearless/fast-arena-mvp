
using FastArena.Core.Domain;
using FastArena.Core.Domain.Activities;
using FastArena.Core.Domain.Activities.Actions;
using FastArena.Core.Domain.Activities.Datas;
using FastArena.Core.Domain.Effects;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.MondterFights;
using FastArena.Core.Domain.Monsters;
using FastArena.Core.Exceptions;
using FastArena.Core.Interfaces.App;
using FastArena.Core.Models;
using FastArena.Core.Services.Effects;

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
    private readonly EffectHandlerRegistry _effectHandlerRegistry;

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
        IActivitySessionService activitySessionService,
        EffectHandlerRegistry effectHandlerRegistry
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
        _effectHandlerRegistry = effectHandlerRegistry;
    }

    public async Task<MonsterFightRoundResult> CalcRoundAsync(MonsterFightActionPayload payload, Guid userId)
    {
        var user = await _userService.GetAsync(userId);
        ValidateUser(userId, user);
        var activityState = await _activityStateService.GetByHeroIdAsync(user.SelectedHeroId!.Value);
        ValidateActivityState(activityState);

        return await ActRound(payload, activityState!);
    }

    private async Task<MonsterFightRoundResult> ActRound(
        MonsterFightActionPayload payload,
        ActivitySession activityState
        )
    {
        var sessionLock = await _activityStateService.GetLockerForSessionIdAsync(activityState.Id);
        if (!await sessionLock.WaitAsync(0))
            throw new ActionDeniedException($"The activity with id {activityState.Id} is already acting right now!");

        var cleanTheLocker = false;
        var activityStateId = activityState.Id;

        try
        {
            var monsterFight = await BuildMonsterFight(activityState);
            var lastState = GetTheLastState(monsterFight);
            ValidateActVariant(payload.ActVariant, lastState.Value);
            
            switch (payload.ActVariant)
            {
                case HeroActVariant.ATTACK:
                    await DoAttack(monsterFight, activityState);
                    break;
                case HeroActVariant.USE_ITEM:
                    await DoUseItemAndAttack(monsterFight, activityState, payload.ActionData);
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
        await DoAttackInternal(monsterFight, activityState, isHeroPassive: false, consumedItem: null);
    }

    private async Task DoUseItemAndAttack(MonsterFight monsterFight, ActivitySession activityState, MonsterFightActionData? actionData)
    {
        if (actionData?.UsedPocketItemCellId == null)
        {
            throw new InvalidOperationException("usedPocketItemCellId is required for USE_ITEM action.");
        }

        var consumedItem = await _heroService.ConsumePocketItemForFightAsync(monsterFight.Hero.Id, actionData.UsedPocketItemCellId.Value);
        await DoAttackInternal(monsterFight, activityState, isHeroPassive: true, consumedItem: consumedItem);
    }

    private async Task DoAttackInternal(
        MonsterFight monsterFight,
        ActivitySession activityState,
        bool isHeroPassive,
        HeroItemCell? consumedItem)
    {
        var lastState = GetTheLastState(monsterFight);
        var lastStateValue = lastState.Value;

        var workingState = new MonsterFightActionState
        {
            HeroHealth = lastStateValue.HeroHealth,
            HeroAbility = lastStateValue.HeroAbility,
            HeroDiceRoll = null,
            MonsterHealth = lastStateValue.MonsterHealth,
            MonsterAbility = lastStateValue.MonsterAbility,
            MonsterDiceRoll = null,
            StrikeStrength = 0,
            Result = null,
            ActVariants = new HashSet<HeroActVariant>(),
            ActiveEffects = CloneActiveEffects(lastStateValue.ActiveEffects),
            PocketItems = ClonePocketItems(lastStateValue.PocketItems),
        };

        CleanupExpiredEffects(workingState);

        if (consumedItem != null)
        {
            ActivateItemEffects(workingState, consumedItem);
            workingState.PocketItems = RemoveConsumedPocketItem(workingState.PocketItems, consumedItem.Id);
        }

        ApplyRoundStartEffects(workingState, monsterFight.Hero, monsterFight.Monster);

        var rnd = new Random();
        var heroDiceRoll = rnd.Next(1, 7);
        var monsterDiceRoll = rnd.Next(1, 7);

        workingState.HeroDiceRoll = heroDiceRoll;
        workingState.MonsterDiceRoll = monsterDiceRoll;

        ApplyStrikeClaimedEffects(workingState, monsterFight.Hero, monsterFight.Monster);

        var diceRollBalance = heroDiceRoll - monsterDiceRoll;
        var abilityBalance = (int)((workingState.HeroAbility - workingState.MonsterAbility) / 3);
        abilityBalance = Math.Abs(abilityBalance) > 3 ? (abilityBalance / Math.Abs(abilityBalance)) * 3 : abilityBalance;

        var roundResultType = MonsterFightActionStateResultType.DRAW;
        workingState.StrikeStrength = 0;

        if (diceRollBalance < 0 && (diceRollBalance + abilityBalance) < 0)
        {
            roundResultType = MonsterFightActionStateResultType.STRIKE_BY_MONSTER;
            workingState.StrikeStrength = diceRollBalance;
            if (abilityBalance > 0)
            {
                workingState.StrikeStrength += abilityBalance;
            }
            workingState.StrikeStrength = Math.Abs(workingState.StrikeStrength);
        }

        if (diceRollBalance > 0 && (diceRollBalance + abilityBalance) > 0)
        {
            if (!isHeroPassive)
            {
                roundResultType = MonsterFightActionStateResultType.STRIKE_BY_HERO;
                workingState.StrikeStrength = diceRollBalance;
                if (abilityBalance < 0)
                {
                    workingState.StrikeStrength += abilityBalance;
                }
                workingState.StrikeStrength = Math.Abs(workingState.StrikeStrength);
            }
        }

        var hitZone = HitZone.HEAD;
        if (workingState.StrikeStrength > 0)
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

        if (roundResultType == MonsterFightActionStateResultType.STRIKE_BY_HERO && workingState.StrikeStrength > 0)
        {
            ApplyPowerModifierEffects(workingState, monsterFight.Hero, monsterFight.Monster);

            if (workingState.StrikeStrength < 1)
            {
                workingState.StrikeStrength = 0;
                roundResultType = MonsterFightActionStateResultType.DRAW;
            }
        }

        var newHeroHealth = workingState.HeroHealth;
        var newMonsterHealth = workingState.MonsterHealth;

        if (roundResultType == MonsterFightActionStateResultType.STRIKE_BY_MONSTER)
        {
            newHeroHealth = GetNewHeroHealth(workingState, hitZone);
        }
        else if (roundResultType == MonsterFightActionStateResultType.STRIKE_BY_HERO)
        {
            newMonsterHealth = GetNewMonsterHealth(workingState, hitZone);
        }

        workingState.HeroHealth = newHeroHealth;
        workingState.MonsterHealth = newMonsterHealth;
        workingState.HeroAbility = workingState.HeroHealth / 10;
        workingState.MonsterAbility = workingState.MonsterHealth / 10;

        ApplyRoundEndEffects(workingState, monsterFight.Hero, monsterFight.Monster);
        DecrementEffectDurations(workingState);

        var newState = await BuildNewMonsterFightState(
                monsterFight,
                lastState,
                roundResultType,
                workingState,
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

        await _activityStateService.UpdateStateAsync(activityState);
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
        MonsterFightActionState workingState,
        HitZone hitZone)
    {
        var newOrder = lastState.Key + 1;

        var newHeroHealth = workingState.HeroHealth;
        var newMonsterHealth = workingState.MonsterHealth;
        var newHeroAbility = workingState.HeroAbility;
        var newMonsterAbility = workingState.MonsterAbility;

        var damage = 0;
        if (roundResultType == MonsterFightActionStateResultType.STRIKE_BY_MONSTER)
            damage = lastState.Value.HeroHealth - newHeroHealth;
        if (roundResultType == MonsterFightActionStateResultType.STRIKE_BY_HERO)
            damage = lastState.Value.MonsterHealth - newMonsterHealth;

        var newState = new MonsterFightActionState
        {
            HeroHealth = newHeroHealth,
            HeroAbility = newHeroAbility,
            HeroDiceRoll = workingState.HeroDiceRoll,
            MonsterHealth = newMonsterHealth,
            MonsterAbility = newMonsterAbility,
            MonsterDiceRoll = workingState.MonsterDiceRoll,
            StrikeStrength = workingState.StrikeStrength,
            ActVariants = BuildNextActVariants(newHeroHealth, newMonsterHealth, workingState.PocketItems),
            ActiveEffects = CloneActiveEffects(workingState.ActiveEffects),
            PocketItems = ClonePocketItems(workingState.PocketItems),
            Result = await BuildMonsterFightActionStateResult(
                monsterFight, 
                workingState.StrikeStrength,
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
        MonsterFightActionState state,
        HitZone hitZone)
    {
        var damage = state.StrikeStrength * _damageMap[hitZone];
        return state.HeroHealth - damage;
    }

    private int GetNewMonsterHealth(
        MonsterFightActionState state,
        HitZone hitZone)
    {
        var damage = state.StrikeStrength * _damageMap[hitZone];
        return state.MonsterHealth - damage;
    }

    private HashSet<HeroActVariant> BuildNextActVariants(int heroHealth, int monsterHealth, List<HeroItemCell> pocketItems)
    {
        if (heroHealth <= 0 || monsterHealth <= 0)
        {
            return new HashSet<HeroActVariant> { HeroActVariant.FINALIZE };
        }

        var result = new HashSet<HeroActVariant> { HeroActVariant.ATTACK };
        if (pocketItems.Count > 0)
        {
            result.Add(HeroActVariant.USE_ITEM);
        }

        return result;
    }

    private static List<ActiveEffect> CloneActiveEffects(List<ActiveEffect> effects)
    {
        return effects.Select(e => new ActiveEffect
        {
            DefinitionId = e.DefinitionId,
            Type = e.Type,
            RemainingRounds = e.RemainingRounds,
            Magnitude = e.Magnitude,
            MinValue = e.MinValue,
            MaxValue = e.MaxValue,
            ChancePercent = e.ChancePercent,
            ConditionType = e.ConditionType,
            TargetType = e.TargetType,
            Priority = e.Priority,
            StackCount = e.StackCount,
        }).ToList();
    }

    private static List<HeroItemCell> ClonePocketItems(List<HeroItemCell> items)
    {
        return items.Select(i => new HeroItemCell
        {
            Id = i.Id,
            HeroId = i.HeroId,
            ItemId = i.ItemId,
            Amount = i.Amount,
            Item = i.Item,
        }).ToList();
    }

    private List<HeroItemCell> RemoveConsumedPocketItem(List<HeroItemCell> items, Guid heroItemCellId)
    {
        var pocketItem = items.FirstOrDefault(i => i.Id == heroItemCellId);
        if (pocketItem == null)
        {
            return items;
        }

        if (pocketItem.Amount <= 1)
        {
            return items.Where(i => i.Id != heroItemCellId).ToList();
        }

        pocketItem.Amount -= 1;
        return items;
    }

    private void ActivateItemEffects(MonsterFightActionState state, HeroItemCell consumedItem)
    {
        var effects = consumedItem.Item?.Effects ?? new List<EffectDefinition>();
        foreach (var definition in effects.OrderBy(e => e.Priority))
        {
            var existing = state.ActiveEffects.FirstOrDefault(e => e.Type == definition.Type && e.RemainingRounds > 0);
            if (existing == null)
            {
                state.ActiveEffects.Add(new ActiveEffect
                {
                    DefinitionId = definition.Id,
                    Type = definition.Type,
                    RemainingRounds = definition.DurationRounds,
                    Magnitude = definition.Magnitude,
                    MinValue = definition.MinValue,
                    MaxValue = definition.MaxValue,
                    ChancePercent = definition.ChancePercent,
                    ConditionType = definition.ConditionType,
                    TargetType = definition.TargetType,
                    Priority = definition.Priority,
                    StackCount = 1,
                });
                continue;
            }

            var handler = _effectHandlerRegistry.GetHandler(definition.Type);
            handler.Stack(existing, definition);
        }
    }

    private void ApplyRoundStartEffects(MonsterFightActionState state, Hero hero, Monster monster)
    {
        foreach (var effect in state.ActiveEffects.OrderBy(e => e.Priority))
        {
            _effectHandlerRegistry.GetHandler(effect.Type).OnRoundStart(effect, state, hero, monster);
        }
    }

    private void ApplyStrikeClaimedEffects(MonsterFightActionState state, Hero hero, Monster monster)
    {
        foreach (var effect in state.ActiveEffects.OrderBy(e => e.Priority))
        {
            _effectHandlerRegistry.GetHandler(effect.Type).OnStrikeClaimed(effect, state, hero, monster);
        }
    }

    private void ApplyPowerModifierEffects(MonsterFightActionState state, Hero hero, Monster monster)
    {
        foreach (var effect in state.ActiveEffects.OrderBy(e => e.Priority))
        {
            _effectHandlerRegistry.GetHandler(effect.Type).OnPowerModifiers(effect, state, hero, monster);
        }
    }

    private void ApplyRoundEndEffects(MonsterFightActionState state, Hero hero, Monster monster)
    {
        foreach (var effect in state.ActiveEffects.OrderBy(e => e.Priority))
        {
            _effectHandlerRegistry.GetHandler(effect.Type).OnRoundEnd(effect, state, hero, monster);
        }
    }

    private static void DecrementEffectDurations(MonsterFightActionState state)
    {
        foreach (var effect in state.ActiveEffects)
        {
            effect.RemainingRounds -= 1;
        }
    }

    private static void CleanupExpiredEffects(MonsterFightActionState state)
    {
        state.ActiveEffects = state.ActiveEffects.Where(e => e.RemainingRounds > 0).ToList();
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

        if (activitySession != null && await _activitySessionService.HasNextAsync(activitySession))
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
            throw new Exception("There is no any activity sessions for selected user hero.");
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