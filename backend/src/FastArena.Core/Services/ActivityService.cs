using FastArena.Core.Domain.Activities;
using FastArena.Core.Domain.Activities.Actions;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Interfaces.App;

namespace FastArena.Core.Services;

public class ActivityService : IActivityService
{
    private readonly IHeroService _heroService;
    private readonly List<Activity> CommonActivities = [
        new Activity {
            Id = Guid.Parse("c6f7c1ad-4e88-4272-814e-94400ac11bdf"),
            Title = "Сразится со слабым монстром",
            DangerLevel = ActivityDangerLevel.LOW,
            AwardLevel = ActivityAwardLevel.LOW,
            Type = ActivityType.REGULAR,
            ActivationTitle = "Сразиться",
            IconUrl = "/img/activities/vile-fluid.svg",
            Description = "Сразиться со случайным слабым монстром.\n\r С выской долей вероятности монстр будет слабее героя. Награда точно будет низкой.",
            Actions = [
                new MonsterFightAction() {
                    Id = Guid.NewGuid(),
                    Title = "Драка с монстром!",
                    Type = ActivityActionType.MONSTER_FIGHT,
                }
            ],
        }, 
        new Activity {
            Id = Guid.Parse("abad6339-64d9-4830-8d8f-66091ca54525"),
            Title = "Сразится с обычным монстром",
            DangerLevel = ActivityDangerLevel.MEDIUM,
            AwardLevel = ActivityAwardLevel.MEDIUM,
            Type = ActivityType.REGULAR,
            ActivationTitle = "Сразиться",
            IconUrl = "/img/activities/vile-fluid.svg",
            Description = "Сразиться со случайным монстром.\n\r С выской долей вероятности монстр будет близок по силе к герою. Награда точно будет средней.",
            Actions = [
                new MonsterFightAction() {
                    Id = Guid.NewGuid(),
                    Title = "Драка с монстром!",
                    Type = ActivityActionType.MONSTER_FIGHT,
                }
            ],
        },
        new Activity {
            Id = Guid.Parse("42c6f9cf-8f4b-4012-b27c-218f79849a7b"),
            Title = "Сразится с сильным монстром",
            DangerLevel = ActivityDangerLevel.HIGH,
            AwardLevel = ActivityAwardLevel.HIGH,
            Type = ActivityType.REGULAR,
            ActivationTitle = "Сразиться",
            IconUrl = "/img/activities/vile-fluid.svg",
            Description = "Сразиться со случайным монстром.\n\r С выской долей вероятности монстр будет сильнее героя. Награда точно будет высокой.",
            Actions = [
                new MonsterFightAction() {
                    Id = Guid.NewGuid(),
                    Title = "Драка с монстром!",
                    Type = ActivityActionType.MONSTER_FIGHT,
                }
            ],
        },
    ];

    public ActivityService(IHeroService heroService)
    {
        _heroService = heroService;
    }

    public async Task<ICollection<Activity>> GetForSelectedHeroByUserId(Guid userId)
    {
        var hero = await _heroService.GetSelectedByUserIdAsync(userId);
        if (hero == null)
        {
            throw new ArgumentException("There is no selected hero");
        }

        return await GetByHero(hero);
    }

    public async Task<ICollection<Activity>> GetByHero(Hero hero)
    {
        var commonActivities = await GetCommonActivitiesForHero(hero);
        var specialActivities = await GetSpecialActivitiesForHero(hero);

        return commonActivities.Concat(specialActivities).ToList();
    }

    public async Task<bool> IsActivityAvailableForHero(Guid activityId, Hero hero)
    {
        var activities = await GetByHero(hero);

        return activities.Any(a => a.Id == activityId);
    }

    public async Task<Activity> GetAsync(Guid id)
    {
        var allActivities = CommonActivities;
        return await Task.FromResult(allActivities.First(a => a.Id == id));
    }

    #region private methods
    private async Task<ICollection<Activity>> GetCommonActivitiesForHero(Hero hero)
    {
        if (!await CheckIsCommonActivitiesAvaliable(hero))
        {
            return await Task.FromResult(new List<Activity>());
        }

        return CommonActivities;
    }

    private async Task<bool> CheckIsCommonActivitiesAvaliable(Hero hero)
    {
        return await Task.FromResult(true);
    }

    private async Task<ICollection<Activity>> GetSpecialActivitiesForHero(Hero hero)
    {
        return await Task.FromResult(new List<Activity>());
    }
    #endregion private methods
}
