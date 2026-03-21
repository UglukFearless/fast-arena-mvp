using FastArena.Core.Interfaces.App;
using FastArena.WebApi.Dtos;
using FastArena.WebApi.Profiles;
using FastArena.WebApi.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastArena.WebApi.Controllers;

[Route("api/[controller]")]
[Authorize]
public class ActivitySessionController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IActivitySessionService _activitySessionService;

    public ActivitySessionController(
        IHttpContextAccessor httpContextAccessor, 
        IActivitySessionService activitySessionService
        )
    {
        _httpContextAccessor = httpContextAccessor;
        _activitySessionService = activitySessionService;
    }

    [HttpPost("start")]
    public async Task<ActionResult<ActivitySessionDto>> Start(Guid activityId)
    {
        var userId = AuthProvider.GetCurrentUserIdFromAccessor(_httpContextAccessor);
        var session = await _activitySessionService.StartActivityAsync(activityId, userId);
        return Ok(ActivitySessionProfile.Map(session));
    }

    [HttpGet("current")]
    public async Task<ActionResult<ActivitySessionDto?>> GetCurrent()
    {
        var userId = AuthProvider.GetCurrentUserIdFromAccessor(_httpContextAccessor);
        var session = await _activitySessionService.GetCurrentActivityByUserIdAsync(userId);
        return Ok(ActivitySessionProfile.Map(session));
    }
}
