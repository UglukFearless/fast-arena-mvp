using FastArena.Core.Interfaces.App;
using FastArena.WebApi.Dtos;
using FastArena.WebApi.Models;
using FastArena.WebApi.Profiles;
using FastArena.WebApi.Providers;
using Microsoft.AspNetCore.Mvc;

namespace FastArena.WebApi.Controllers;

[Route("api/[controller]")]
public class AccountController : ControllerBase
{

    private IUserService _userService;
    private AuthProvider _authProvider;

    public AccountController(IUserService userService, AuthProvider authProvider) 
    {
        _userService = userService;
        _authProvider = authProvider;
    }

    [HttpPost("registration")]
    [Produces("application/json")]
    public async Task<ActionResult<AuthResultDto>> Registration([FromBody] RegistrationModel registrationModel)
    {
        var login = registrationModel.Login.Trim();
        var password = registrationModel.Password.Trim();

        if (!IsValidRegistrationData(login, password, out string errorText))
        {
            return BadRequest($"Invalid registration data: {errorText}");
        }


        var user = await _userService.CreateAsyc(login, password);
        var token = _authProvider.GetTokenForUser(user);

        var response = new AuthResultDto(token, UserProfile.Map(user));
        return Ok(response);
    }

    [HttpPost("login")]
    [Produces("application/json")]
    public async Task<ActionResult<AuthResultDto>> Login([FromBody] AuthModel authModel)
    {
        var login = authModel.Login.Trim();
        var password = authModel.Password.Trim();

        var user = await _userService.GetByLoginAndPasswordAsync(login, password);

        if (user == null)
        {
            return Unauthorized("Login or password is wrong.");
        }
        var token = _authProvider.GetTokenForUser(user);

        var response = new AuthResultDto(token, UserProfile.Map(user));
        return Ok(response);
    }

    private bool IsValidRegistrationData(string login, string password, out string errorText)
    {
        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
        {
            errorText = "Login or Password is null or white space.";
            return false;
        }

        if (login.Contains(' ') || password.Contains(' '))
        {
            errorText = "Login or Password contains space symbols.";
            return false;
        }

        errorText = null;
        return true;
    }
}
