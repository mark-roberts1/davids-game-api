using Davids.Game.Models.Pools;

namespace Davids.Game.Models.UserPools;
public class UserPoolResponse : PoolResponse
{
    public long UserPoolId { get; set; }
    public long UserId { get; set; }
    public UserPoolAttribute Attributes { get; set; }

    public bool IsAdmin => Attributes.HasFlag(UserPoolAttribute.Admin) || IsOwner;
    public bool IsOwner => Attributes.HasFlag(UserPoolAttribute.Owner);
    public bool IsBlocked => Attributes.HasFlag(UserPoolAttribute.Blocked);
    public bool IsDeleted => Attributes.HasFlag(UserPoolAttribute.Deleted);
}
