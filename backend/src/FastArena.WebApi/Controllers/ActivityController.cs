
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
public class ActivityController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IActivityService _activityService;
    public ActivityController(IHttpContextAccessor httpContextAccessor, IActivityService activityService)
    {
        _httpContextAccessor = httpContextAccessor;
        _activityService = activityService;
    }
    public async Task<ActionResult<IList<ActivityDto>>> Get()
    {
        var userId = AuthProvider.GetCurrentUserIdFromAccessor(_httpContextAccessor);
        var activities = await _activityService.GetForSelectedHeroByUserId(userId);
        return Ok(ActivityProfile.Map(activities.ToList()));
    }
}
