using FastArena.Core.Interfaces.App;
using FastArena.WebApi.Dtos;
using FastArena.WebApi.Models;
using FastArena.WebApi.Profiles;
using FastArena.WebApi.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastArena.WebApi.Controllers;

[Route("api/[controller]")]
[Authorize]
public class StatisticController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IStatisticService _statisticService;

    public StatisticController(
        IHttpContextAccessor httpContextAccessor, 
        IStatisticService statisticService)
    {
        _httpContextAccessor = httpContextAccessor;
        _statisticService = statisticService;
    }

    [HttpGet]
    public async Task<ActionResult<StatisticDataDto>> Get(StatisticFilterModel filter)
    {
        var userId = AuthProvider.GetCurrentUserIdFromAccessor(_httpContextAccessor);
        var data = await _statisticService.GetAsync(StatisticProfile.Map(filter), userId);
        return Ok(StatisticProfile.Map(data));
    }
}
