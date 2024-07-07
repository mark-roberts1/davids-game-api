using Davids.Game.Models.UserPools;
using Davids.Game.Models.Users;
using System.Security.Claims;

namespace Davids.Game.Users;

public interface IUserService
{
    public Task<UserResponse> GetUserAsync(ClaimsPrincipal loggedInUser, CancellationToken cancellationToken);
    public Task<UserPoolResponse?> GetUserPoolAsync(long userId, long poolId, CancellationToken cancellationToken);
    public Task<UserPoolResponse?> GetUserPoolAsync(long userPoolId, CancellationToken cancellationToken);
}
