namespace Davids.Game.Data;

public partial class Team
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? Code { get; set; }

    public string? LogoLink { get; set; }

    public short? CountryId { get; set; }

    public long? SourceId { get; set; }

    public virtual Country? Country { get; set; }

    public virtual ICollection<ListEntry> ListEntries { get; set; } = new List<ListEntry>();

    public virtual ICollection<TeamSeasonLeague> TeamSeasonLeagues { get; set; } = new List<TeamSeasonLeague>();

    public virtual ICollection<TeamStatistic> TeamStatistics { get; set; } = new List<TeamStatistic>();

    public virtual ICollection<Venue> Venues { get; set; } = new List<Venue>();
}
