using FastArena.Core.Domain.Activities.Actions;

namespace FastArena.Core.Domain.Activities;

public class Activity
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string ActivationTitle { get; set; }
    public string? Description { get; set; }
    public ActivityDangerLevel DangerLevel { get; set; }
    public ActivityAwardLevel AwardLevel { get; set; }
    public ActivityType Type { get; set; }
    public required string IconUrl { get; set; }
    public required ICollection<ActivityAction> Actions { get; set; }
}
