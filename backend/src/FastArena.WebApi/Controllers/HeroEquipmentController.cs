using FastArena.Core.Exceptions;
using FastArena.Core.Interfaces.App;
using FastArena.WebApi.Models;
using FastArena.WebApi.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastArena.WebApi.Controllers;

[Route("api/hero-equipment")]
[ApiController]
public class HeroEquipmentController : ControllerBase
{
    private readonly IHeroEquipmentService _heroEquipmentService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HeroEquipmentController(IHeroEquipmentService heroEquipmentService, IHttpContextAccessor httpContextAccessor)
    {
        _heroEquipmentService = heroEquipmentService;
        _httpContextAccessor = httpContextAccessor;
    }

    [Authorize]
    [HttpPost("equip")]
    public async Task<ActionResult> Equip([FromBody] HeroEquipModel model)
    {
        try
        {
            var userId = AuthProvider.GetCurrentUserIdFromAccessor(_httpContextAccessor);
            await _heroEquipmentService.EquipAsync(userId, model.HeroItemCellId);
            return Ok();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ActionDeniedException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [Authorize]
    [HttpPost("unequip")]
    public async Task<ActionResult> Unequip([FromBody] HeroUnequipModel model)
    {
        try
        {
            var userId = AuthProvider.GetCurrentUserIdFromAccessor(_httpContextAccessor);
            await _heroEquipmentService.UnequipAsync(userId, model.Slot);
            return Ok();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ActionDeniedException ex)
        {
            return Conflict(ex.Message);
        }
    }
}