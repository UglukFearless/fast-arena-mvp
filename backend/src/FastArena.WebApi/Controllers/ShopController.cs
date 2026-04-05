using FastArena.Core.Exceptions;
using FastArena.Core.Interfaces.App;
using FastArena.Core.Models;
using FastArena.WebApi.Dtos;
using FastArena.WebApi.Profiles;
using ShopTransactionRequestModel = FastArena.WebApi.Models.ShopTransactionModel;
using FastArena.WebApi.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastArena.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShopController : ControllerBase
{
    private readonly IShopService _shopService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ShopController(IShopService shopService, IHttpContextAccessor httpContextAccessor)
    {
        _shopService = shopService;
        _httpContextAccessor = httpContextAccessor;
    }

    [Authorize]
    [HttpGet("catalog")]
    public async Task<ActionResult<ShopCatalogDto>> GetShopCatalog()
    {
        try
        {
            var userId = AuthProvider.GetCurrentUserIdFromAccessor(_httpContextAccessor);
            var catalog = await _shopService.GetAvailableItemsAsync(userId);
            return Ok(ShopCatalogProfile.Map(catalog));
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
    [HttpGet("hero-items")]
    public async Task<ActionResult<List<ShopHeroItemDto>>> GetHeroItems()
    {
        try
        {
            var userId = AuthProvider.GetCurrentUserIdFromAccessor(_httpContextAccessor);
            var items = await _shopService.GetHeroItemsAsync(userId);
            return Ok(ShopHeroItemProfile.Map(items));
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
    [HttpPost("transaction")]
    public async Task<ActionResult> ExecuteTransaction([FromBody] ShopTransactionRequestModel model)
    {
        try
        {
            var userId = AuthProvider.GetCurrentUserIdFromAccessor(_httpContextAccessor);
            await _shopService.ExecuteTransactionAsync(userId, new ShopTransactionModel
            {
                SellItems = model.SellItems.Select(i => new HeroItemTakeRequest
                {
                    HeroItemCellId = i.HeroItemCellId,
                    Quantity = i.Quantity,
                }).ToList(),
                BuyItems = model.BuyItems.Select(i => new ShopBuyRequestItem
                {
                    ItemId = i.ItemId,
                    Quantity = i.Quantity,
                }).ToList(),
            });
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
