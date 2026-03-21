
using FastArena.Core.Domain.Activities.Actions;
using FastArena.Core.Domain.Activities.Datas;

namespace FastArena.WebApi.Dtos;

public class ActivityActionCaseDto
{
    public required ActivityActionType Type { get; set; }
}
