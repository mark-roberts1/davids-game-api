using Davids.Game.Api.DiscordAuth;
using Microsoft.AspNetCore.Mvc;

namespace Davids.Game.Api.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpGet, Route("me"), DiscordAuthorize]
    public async Task<IActionResult> GetSelfAsync(CancellationToken cancellationToken)
    {
        var user = this.User;

        return Ok(this.User.Claims.ToDictionary(c => c.Type, c => c.Value));
    }
}
