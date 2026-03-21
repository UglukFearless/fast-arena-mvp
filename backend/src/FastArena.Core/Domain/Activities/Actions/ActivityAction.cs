using FastArena.Core.Domain.Heroes;
using FastArena.Core.Interfaces.App;

namespace FastArena.Core.Domain.Activities.Actions;

public abstract class ActivityAction
{
    public Guid Id { get; init; }
    public ActivityActionType Type { get; init; }
    public required string Title { get; init; }
    public abstract Task<ActivityActionCase> СreateCaseAsync(Hero hero, Activity context, IMonsterService monsterService);
}
