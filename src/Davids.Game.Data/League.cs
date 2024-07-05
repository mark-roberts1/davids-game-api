namespace Davids.Game.Data;

public partial class League
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public short Type { get; set; }

    public string? LogoLink { get; set; }

    public short? CountryId { get; set; }

    public long? SourceId { get; set; }

    public virtual Country? Country { get; set; }

    public virtual ICollection<Pool> Pools { get; set; } = new List<Pool>();

    public virtual ICollection<TeamSeasonLeague> TeamSeasonLeagues { get; set; } = new List<TeamSeasonLeague>();
}
