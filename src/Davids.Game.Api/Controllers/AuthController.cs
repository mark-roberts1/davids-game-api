using AutoMapper;
using Davids.Game.Api.DiscordAuth;
using Davids.Game.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Davids.Game.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(DiscordHttpClient discord, IMapper mapper, IConfiguration configuration) : ControllerBase
{
    [HttpGet, Route("discord")]
    public IActionResult GetDiscordAuthAddressAsync([FromQuery] string redirectUrl)
    {
        var baseUrl = configuration.GetValue<string>("DiscordAuth:AuthEndpoint");
        var clientId = configuration.GetValue<string>("DISCORD_CLIENT_ID")!;
        var redirectUrlParameter = Uri.EscapeDataString(redirectUrl);

        return Ok(new DiscordAuthorizeRoute
        {
            AuthorizeUrl = $"{baseUrl}?client_id={clientId}&response_type=code&redirect_uri={redirectUrlParameter}&scope=identify"
        });
    }

    [HttpPost, Route("discord")]
    [ProducesResponseType<TokenResponse>(200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> DiscordAuthAsync([FromQuery] string code, [FromQuery] string redirectUrl, CancellationToken cancellationToken)
    {
        try
        {
            var discordToken = await discord.GetAccessTokenAsync(code, redirectUrl, cancellationToken);

            return Ok(mapper.Map<TokenResponse>(discordToken));
        }
        catch
        {
            return Unauthorized();
        }
    }

    [HttpPost, Route("discord/refresh")]
    [ProducesResponseType<TokenResponse>(200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> RefreshDiscordAuthAsync([FromQuery] string refreshToken, CancellationToken cancellationToken)
    {
        try
        {
            var discordToken = await discord.RefreshAccessTokenAsync(refreshToken, cancellationToken);

            return Ok(mapper.Map<TokenResponse>(discordToken));
        }
        catch
        {
            return Unauthorized();
        }
    }

    [HttpPost, Route("discord/logout"), DiscordAuthorize]
    public async Task<IActionResult> LogoutAsync(CancellationToken cancellationToken)
    {
        await discord.RevokeTokenAsync(this.User.Claims.ToDictionary(c => c.Type, c => c.Value)["auth_token"], cancellationToken);

        return Ok();
    }
}
