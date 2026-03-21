using FastArena.Core.Interfaces.App;
using FastArena.WebApi.Dtos;
using FastArena.WebApi.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastArena.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PortraitController : ControllerBase
{
    private readonly IPortraitService _portraitService;
    public PortraitController(IPortraitService portraitService)
    {
        _portraitService = portraitService;
    }

    [Authorize]
    [HttpGet("all-heroes")]
    public async Task<ActionResult<IList<PortraitDto>>> GetAllForHeroes()
    {
        var portraits = await _portraitService.GetAllForHeroes();
        return Ok(PortraitProfile.Map(portraits.ToList()));
    }
}
