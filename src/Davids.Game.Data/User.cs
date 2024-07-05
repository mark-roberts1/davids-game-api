namespace Davids.Game.Data;

public partial class User
{
    public long Id { get; set; }

    public short IdentityProviderId { get; set; }

    public string ExternalId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual IdentityProvider IdentityProvider { get; set; } = null!;

    public virtual ICollection<UserPool> UserPools { get; set; } = new List<UserPool>();
}
