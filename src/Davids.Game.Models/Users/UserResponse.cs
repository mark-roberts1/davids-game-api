using Davids.Game.Models.UserPools;

namespace Davids.Game.Models.Users;

public class UserResponse
{
    public long Id { get; set; }

    public short IdentityProviderId { get; set; }

    public string ExternalId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Avatar { get; set; } = null!;
}

public class PoolUserResponse : UserResponse
{
    public long UserPoolId { get; set; }
    public UserPoolAttribute Attributes { get; set; }

    public bool IsAdmin => Attributes.HasFlag(UserPoolAttribute.Admin) || IsOwner;
    public bool IsOwner => Attributes.HasFlag(UserPoolAttribute.Owner);
    public bool IsBlocked => Attributes.HasFlag(UserPoolAttribute.Blocked);
    public bool IsDeleted => Attributes.HasFlag(UserPoolAttribute.Deleted);
}