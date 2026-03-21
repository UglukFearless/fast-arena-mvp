using FastArena.Core.Domain.Activities;
using FastArena.Core.Domain.Activities.Actions;
using FastArena.WebApi.Dtos;

namespace FastArena.WebApi.Profiles;

internal class ActivitySessionProfile
{
    public static ActivitySessionDto? Map(ActivitySession session)
    {
        if (session == null)
            return null;

        return new ActivitySessionDto
        {
            Id = session.Id,
            CurrentAction = Map(session.CurrentAction),
        };
    }

    public static ActivityActionCaseDto Map(ActivityActionCase actionCase)
    {
        return new ActivityActionCaseDto
        {
            Type = actionCase.Type,
        };
    }
}
