using Davids.Game.Models.Lists;
using Davids.Game.Models.Pools;
using Davids.Game.Models.UserPools;
using Davids.Game.Models.Users;

namespace Davids.Game.Pools;

public interface IPoolsRepository
{
    Task<PoolResponse?> GetPoolAsync(long poolId, CancellationToken cancellationToken);
    Task<long> CreatePoolAsync(PoolWriteRequest request, CancellationToken cancellationToken);
    Task<bool> UpdatePoolAsync(long poolId, PoolWriteRequest request, CancellationToken cancellationToken);
    Task<bool> DeletePoolAsync(long poolId, CancellationToken cancellationToken);

    Task<IEnumerable<PoolUserResponse>> GetUsersAsync(long poolId, CancellationToken cancellationToken);
    Task<IEnumerable<ListResponse>> GetListsAsync(long poolId, CancellationToken cancellationToken);
    Task<long> AddUserAsync(long poolId, UserPoolWriteRequest request, CancellationToken cancellationToken);
    Task<bool> RemoveUserAsync(long poolId, long userId, CancellationToken cancellationToken);
}
