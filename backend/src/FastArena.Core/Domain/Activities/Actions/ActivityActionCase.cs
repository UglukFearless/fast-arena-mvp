using FastArena.Core.Domain.Activities.Datas;
using FastArena.Core.Domain.Heroes;

namespace FastArena.Core.Domain.Activities.Actions;

public class ActivityActionCase
{
    public ActivityActionType Type { get; set; }
    public required ActivityActionCaseData Data { get; set; }
    public async Task<ActivityActionState> BuildInitStateAsync(Hero hero, ActivityActionCase activityActionCase)
    {
        return await Data.BuildInitStateAsync(hero, activityActionCase);
    }
}
