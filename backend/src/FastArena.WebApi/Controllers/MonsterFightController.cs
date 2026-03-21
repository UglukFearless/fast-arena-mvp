
using FastArena.Core.Domain.Activities.Actions;
using FastArena.Core.Exceptions;
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
public class MonsterFightController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMonsterFightService _monsterFightService;
    public MonsterFightController(IHttpContextAccessor httpContextAccessor, IMonsterFightService monsterFightService)
    {
        _httpContextAccessor = httpContextAccessor;
        _monsterFightService = monsterFightService;
    }

    public async Task<ActionResult<MonsterFightDto>> Get()
    {
        var userId = AuthProvider.GetCurrentUserIdFromAccessor(_httpContextAccessor);
        var monsterFight = await _monsterFightService.GetByUserIdAsync(userId);
        return Ok(MonsterFightProfile.Map(monsterFight));
    }

    [HttpPost("do")]
    public async Task<ActionResult<MonsterFightRoundResultDto>> DoHeroAction(HeroActVariant actVariant)
    {
        try
        {
            var userId = AuthProvider.GetCurrentUserIdFromAccessor(_httpContextAccessor);
            var roundResult = await _monsterFightService.CalcRoundAsync(actVariant, userId);

            return MonsterFightProfile.Map(roundResult);
        } 
        catch (ActionDeniedException ex)
        {
            return Conflict(ex);
        }   
    }
}
