using FastArena.Core.Domain.Activities.Actions;
using FastArena.Core.Domain.Heroes;

namespace FastArena.Core.Domain.Activities.Datas;

public abstract class ActivityActionCaseData 
{
    public abstract Task<ActivityActionState> BuildInitStateAsync(Hero hero, ActivityActionCase activityActionCase);
}
