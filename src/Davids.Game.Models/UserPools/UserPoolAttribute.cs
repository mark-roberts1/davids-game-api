namespace Davids.Game.Models.UserPools;

[Flags]
public enum UserPoolAttribute : long
{
    None = 0,
    Creator = 1 << 0,
    Owner = 1 << 1,
    Admin = 1 << 2,
    FirstUser = UserPoolAttribute.Creator | UserPoolAttribute.Owner | UserPoolAttribute.Admin,
    Deleted = 1 << 3,
    Blocked = 1 << 4,
}
