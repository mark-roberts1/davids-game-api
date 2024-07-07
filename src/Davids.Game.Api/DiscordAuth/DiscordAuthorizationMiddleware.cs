using Microsoft.AspNetCore.Http.Features;
using System.Net;
using System.Security.Claims;

namespace Davids.Game.Api.DiscordAuth;

public class DiscordAuthorizationMiddleware(RequestDelegate next, ILogger<DiscordAuthorizationMiddleware> logger)
{
    private const short DISCORD_ID_PROVIDER = 1;

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
        var attribute = endpoint?.Metadata.GetMetadata<DiscordAuthorizeAttribute>();

        if (attribute != null)
        {
            var tokenHeader = context.Request.Headers.Authorization.LastOrDefault()?.Split(' ');
            var scheme = tokenHeader?.FirstOrDefault();
            var token = tokenHeader?.LastOrDefault();

            if (token == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            var discord = context.RequestServices.GetRequiredService<DiscordHttpClient>();

            try
            {
                var user = await discord.GetUserAsync(token, CancellationToken.None);

                if (user == null)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    await context.Response.WriteAsync("Forbidden");
                    return;
                }

                context.User = new ClaimsPrincipal(new ClaimsIdentity("Discord"));

                context.User.Identities.First().AddClaim(new("identity_provider", DISCORD_ID_PROVIDER.ToString()));
                context.User.Identities.First().AddClaim(new("id", user.Id));
                context.User.Identities.First().AddClaim(new("username", user.Username));
                context.User.Identities.First().AddClaim(new("avatar", user.Avatar));
                context.User.Identities.First().AddClaim(new("access_token", token));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync("Forbidden");
                return;
            }
        }

        await next(context);
    }
}
