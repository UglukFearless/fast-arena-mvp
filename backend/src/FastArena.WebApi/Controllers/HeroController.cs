using FastArena.Core.Domain.Heroes;
using FastArena.Core.Exceptions;
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
[ApiController]
public class HeroController : ControllerBase
{
    private IHeroService _heroService;
    private IHeroInfoService _heroInfoService;
    private IHttpContextAccessor _httpContextAccessor;

    public HeroController(IHeroService heroService,
        IHeroInfoService heroInfoService,
        IHttpContextAccessor httpContextAccessor)
    {
        _heroService = heroService;
        _heroInfoService = heroInfoService;
        _httpContextAccessor = httpContextAccessor;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<HeroDto>> Create([FromBody] HeroCreationModel model)
    {
        var userId = AuthProvider.GetCurrentUserIdFromAccessor(_httpContextAccessor);
        var hero = await _heroService.CreateAsync(new Core.Models.HeroCreationModel {
                UserId = userId,
                Name =model.Name,
                Sex = model.Sex,
                PortraitId = model.PortraitId
            });
        return Ok(HeroProfile.Map(hero, true));
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<HeroDto>>> Get()
    {
        var userId = AuthProvider.GetCurrentUserIdFromAccessor(_httpContextAccessor);
        var heroes = await _heroService.GetAllWithInfoByUserIdAsync(userId);
        return Ok(HeroProfile.Map(heroes.ToList(), true));
    }

    [Authorize]
    [HttpGet("selected")]
    public async Task<ActionResult<HeroDto>> GetSelected()
    {
        var userId = AuthProvider.GetCurrentUserIdFromAccessor(_httpContextAccessor);
        var hero = await _heroService.GetSelectedByUserIdAsync(userId);
        return Ok(HeroProfile.Map(hero));
    }

    [Authorize]
    [HttpPatch("select")]
    public async Task<ActionResult> Select(Guid id)
    {
        try
        {
            var userId = AuthProvider.GetCurrentUserIdFromAccessor(_httpContextAccessor);
            await _heroService.SelectForUserAsync(id, userId);
            return Ok();
        } catch (ActionDeniedException ex) {
            return Conflict(ex.Message);
        }
    }

    [Authorize]
    [HttpPatch("unselect")]
    public async Task<ActionResult> Unselect()
    {
        try
        {
            var userId = AuthProvider.GetCurrentUserIdFromAccessor(_httpContextAccessor);
            await _heroService.UnselectForUserAsync(userId);
            return Ok();
        }
        catch (ActionDeniedException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("info/{id}")]
    public async Task<ActionResult<HeroInfoDto>> GetInfo(Guid id)
    {
        try
        {
            var userId = AuthProvider.GetCurrentUserIdFromAccessor(_httpContextAccessor);
            var info = await _heroInfoService.GetAsync(id, userId);
            return Ok(HeroInfoProfile.Map(info));
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound("Hero is not found.");
        }
    }

}
