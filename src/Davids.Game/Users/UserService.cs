using Davids.Game.DependencyInjection;
using Davids.Game.Models.UserPools;
using Davids.Game.Models.Users;
using System.Security.Claims;

namespace Davids.Game.Users;

[Service<IUserService>]
internal class UserService(IUsersRepository repository) : IUserService
{
    public async Task<UserResponse> GetUserAsync(ClaimsPrincipal loggedInUser, CancellationToken cancellationToken)
    {
        var claims = loggedInUser.Claims.ToDictionary(c => c.Type, c => c.Value);

        var idProvider = short.Parse(claims["identity_provider"]);
        var externalId = claims["id"];

        var user = await repository.GetUserAsync(idProvider, externalId, cancellationToken);

        if (user == null)
        {
            var userId = await repository.CreateUserAsync(
                new()
                {
                    ExternalId = externalId,
                    IdentityProviderId = idProvider,
                    Name = claims["username"],
                    Avatar = claims["avatar"],
                },
                cancellationToken);

            user = await repository.GetUserAsync(userId, cancellationToken);
        }

        return user!;
    }

    public async Task<UserPoolResponse?> GetUserPoolAsync(long userId, long poolId, CancellationToken cancellationToken)
    {
        return await repository.GetUserPoolAsync(userId, poolId, cancellationToken);
    }

    public async Task<UserPoolResponse?> GetUserPoolAsync(long userPoolId, CancellationToken cancellationToken)
    {
        return await repository.GetUserPoolAsync(userPoolId, cancellationToken);
    }
}
