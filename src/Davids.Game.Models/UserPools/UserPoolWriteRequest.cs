namespace Davids.Game.Models.UserPools;

public class UserPoolWriteRequest
{
    public long UserId { get; set; }

    public UserPoolAttribute Attributes { get; set; }
}
