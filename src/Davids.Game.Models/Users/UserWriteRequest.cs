namespace Davids.Game.Models.Users;

public class UserWriteRequest
{
    public short IdentityProviderId { get; set; }

    public string ExternalId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Avatar { get; set; } = null!;
}
