using Davids.Game.Models.Pools;

namespace Davids.Game.Models.UserPools;
public class UserPoolResponse : PoolResponse
{
    public long UserPoolId { get; set; }

    public UserPoolAttribute Attributes { get; set; }
}
