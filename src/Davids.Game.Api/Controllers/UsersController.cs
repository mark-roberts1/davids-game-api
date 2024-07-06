using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Davids.Game.Api.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpGet, Route("me"), Authorize]
    public async Task<IActionResult> GetSelfAsync(CancellationToken cancellationToken)
    {
        var user = this.User;

        return Ok(user);
    }
}
