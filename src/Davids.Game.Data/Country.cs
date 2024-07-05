namespace Davids.Game.Data;

public partial class Country
{
    public short Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? FlagLink { get; set; }

    public virtual ICollection<League> Leagues { get; set; } = new List<League>();

    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
}
