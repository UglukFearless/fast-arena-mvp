using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastArena.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HealthController : ControllerBase
{
    [HttpGet("ping")]
    public ActionResult<string> Ping()
    {
        return new ActionResult<string>("pong");
    }

    [Authorize]
    [HttpGet("secret-ping")]
    public ActionResult<string> SecretPing()
    {
        return new ActionResult<string>("secret pong!");
    }
}
