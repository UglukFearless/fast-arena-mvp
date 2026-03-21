
using FastArena.Core.Domain.Activities.Actions;

namespace FastArena.Core.Domain.Activities;

public class ActivitySession
{
    public Guid Id { get; set; }
    public Guid HeroId { get; set; }
    public Guid ActivityId { get; set; }
    public required ActivityActionCase CurrentAction { get; set; }
    public required ICollection<ActivityActionCase> Actions { get; set; }
    public Dictionary<int, ActivityActionState> State { get; init; } 
        = new Dictionary<int, ActivityActionState>();
}
