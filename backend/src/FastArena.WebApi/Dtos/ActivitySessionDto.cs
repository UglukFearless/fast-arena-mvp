
using FastArena.Core.Domain.Activities.Actions;

namespace FastArena.WebApi.Dtos;

public class ActivitySessionDto
{
    public Guid Id { get; set; }
    public required ActivityActionCaseDto CurrentAction { get; set; }
}
