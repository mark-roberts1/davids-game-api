namespace Davids.Game.Data;

public partial class Pool
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public int? LeagueId { get; set; }

    public string Season { get; set; } = null!;

    public string? DiscordServerId { get; set; }

    public string JoinCode { get; set; } = null!;

    public virtual League? League { get; set; }

    public virtual ICollection<UserPool> UserPools { get; set; } = new List<UserPool>();
}
