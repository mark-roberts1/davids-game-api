using Davids.Game.Models.Pools;
using Davids.Game.Models.Users;

namespace Davids.Game.Models.UserPools;
public class UserPoolResponse
{
    public long Id { get; set; }

    public long PoolId { get; set; }

    public long UserId { get; set; }

    public UserPoolAttributes Attributes { get; set; }

    public UserResponse User { get; set; } = null!;

    public PoolResponse Pool { get; set; } = null!;
}
