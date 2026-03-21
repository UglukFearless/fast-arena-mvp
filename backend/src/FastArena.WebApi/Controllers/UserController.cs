using FastArena.Core.Interfaces.App;
using FastArena.WebApi.Dtos;
using FastArena.WebApi.Profiles;
using FastArena.WebApi.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastArena.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IHeroService _heroService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserController(
        IUserService userService, 
        IHeroService heroService,
        IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _heroService = heroService;
        _httpContextAccessor = httpContextAccessor;
    }

    [Authorize]
    [HttpGet("info")]
    public async Task<ActionResult<UserInfoDto>> GetInfo()
    {
        try
        {
            var userId = AuthProvider.GetCurrentUserIdFromAccessor(_httpContextAccessor);
            var user = await _userService.GetAsync(userId);

            if (user == null)
            {
                return NotFound("Пользователя с текущим Id несуществует.");
            }

            var heroes = await _heroService.GetAllWithInfoByUserIdAsync(userId);

            return Ok(new UserInfoDto
            {
                User = UserProfile.Map(user),
                Heroes = HeroProfile.Map(heroes.ToList(), true),
            });
        } 
        catch (ArgumentNullException)
        {
            return BadRequest("Невозможно определить пользователя.");
        }
    }
}
