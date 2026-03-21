using FastArena.Core.Domain.Activities.Actions;

namespace FastArena.Core.Domain.Activities;

public class ActivityCase
{
    public Guid ActivityId { get; init; }
    public Guid HeroId { get; init; }
    public ICollection<ActivityActionCase> Actions { get; init; }
}
