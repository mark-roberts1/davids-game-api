using Davids.Game.Models.UserPools;
using Davids.Game.Models.Users;

namespace Davids.Game.Users;

public interface IUsersRepository
{
    Task<UserResponse?> GetUserAsync(long userId, CancellationToken cancellationToken);
    Task<UserPoolResponse?> GetUserPoolAsync(long userPoolId, CancellationToken cancellationToken);
    Task<UserPoolResponse?> GetUserPoolAsync(long userId, long poolId, CancellationToken cancellationToken);
    Task<UserResponse?> GetUserAsync(short identityProviderId, string id, CancellationToken cancellationToken);
    Task<long> CreateUserAsync(UserWriteRequest request, CancellationToken cancellationToken);
    Task<bool> UpdateUserAsync(long userId, UserWriteRequest request, CancellationToken cancellationToken);

    Task<IEnumerable<UserPoolResponse>> GetPoolsAsync(long userId, CancellationToken cancellationToken);
}
