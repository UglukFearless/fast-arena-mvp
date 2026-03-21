using FastArena.Core.Domain.Activities;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Exceptions;
using FastArena.Core.Interfaces.App;
using System;
using static System.Collections.Specialized.BitVector32;

namespace FastArena.Core.Services;

public class ActivitySessionService : IActivitySessionService
{
    private readonly IHeroService _heroService;
    private readonly IActivityService _activityService;
    private readonly IActivityStateService _activityStateService;
    private readonly IMonsterService _monsterService;
    
    public ActivitySessionService(
        IHeroService heroService,
        IActivityService activityService,
        IActivityStateService activityStateService,
        IMonsterService monsterService
        )
    {
        _heroService = heroService;
        _activityService = activityService;
        _activityStateService = activityStateService;
        _monsterService = monsterService;
    }

    public async Task<ActivitySession> StartActivityAsync(Guid activityId, Guid userId)
    {
        var hero = await _heroService.GetSelectedByUserIdAsync(userId);
        if (hero == null)
        {
            throw new ArgumentException("There is no selected hero");
        }

        if (await _activityStateService.IsBusyAsync(userId))
        {
            throw new ActionDeniedException("The user is busy and can not start new activity session!");
        }

        if (!await _activityService.IsActivityAvailableForHero(activityId, hero))
        {
            throw new ActionDeniedException($"The activity with id {activityId} doesn't available for hero {hero.Id}");
        }

        var activityCase = await GenerateActivityCaseForHero(activityId, hero);

        var activitySession = await InitSessionByCaseAsync(hero, activityCase);

        return activitySession;
    }

    private async Task<ActivityCase> GenerateActivityCaseForHero(Guid activityId, Hero hero)
    {
        var activity = (await _activityService.GetByHero(hero)).Single(a => a.Id == activityId);
        return new ActivityCase()
        {
            ActivityId = activityId,
            HeroId = hero.Id,
            Actions = activity.Actions
                .Select(async aa => await aa.СreateCaseAsync(hero, activity, _monsterService))
                .Select(t => t.Result)
                .Where(i => i != null)
                .ToList()
        };
    }

    private async Task<ActivitySession> InitSessionByCaseAsync(Hero hero, ActivityCase activityCase)
    {
        var session = new ActivitySession()
        {
            Id = Guid.NewGuid(),
            HeroId = activityCase.HeroId,
            ActivityId = activityCase.ActivityId,
            CurrentAction = activityCase.Actions.First(),
            Actions = activityCase.Actions,
        };

        session.State[0] = await session.CurrentAction.BuildInitStateAsync(hero, session.CurrentAction);

        await _activityStateService.CreateStateAsync(session);

        return session;
    }

    public async Task<ActivitySession?> GetCurrentActivityByUserIdAsync(Guid userId)
    {
        var hero = await _heroService.GetSelectedByUserIdAsync(userId);
        if (hero == null)
        {
            return null;
        }

        return await _activityStateService.GetByHeroIdAsync(hero.Id);
    }

    public async Task<bool> HasNextAsync(ActivitySession activitySession)
    {
        var currentActionIndex = GetCurrentActionIndex(activitySession);
        var actionCount = activitySession.Actions.Count;

        return await Task.FromResult(currentActionIndex < actionCount - 1);
    }

    public async Task<ActivitySession> GoNextAsync(ActivitySession activitySession)
    {
        var nextIndex = GetCurrentActionIndex(activitySession) + 1;
        var nextAction = activitySession.Actions.ToList()[nextIndex];
        var hero = await _heroService.GetAsync(activitySession.HeroId);
        activitySession.CurrentAction = nextAction;
        activitySession.State[0] = await activitySession.CurrentAction.BuildInitStateAsync(
                hero, activitySession.CurrentAction);

        await _activityStateService.UpdateStateAsync(activitySession);

        return activitySession;
    }

    private int GetCurrentActionIndex(ActivitySession activitySession)
    {
        var actionList = activitySession.Actions.ToList();
        for (var i = 0; i < actionList.Count; i++)
        {
            var action = actionList[i];

            if (action == activitySession.CurrentAction)
            {
                return i;
            }
        }
        throw new Exception("Current action activity index is not found.");
    }
}
