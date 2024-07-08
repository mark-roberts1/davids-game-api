namespace Davids.Game.Data;

public partial class League
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public short LeagueTypeId { get; set; }

    public string? LogoLink { get; set; }

    public short? CountryId { get; set; }

    public long? SourceId { get; set; }

    public virtual Country? Country { get; set; }

    public virtual LeagueType LeagueType { get; set; } = null!;

    public virtual ICollection<Pool> Pools { get; set; } = new List<Pool>();

    public virtual ICollection<Season> Seasons { get; set; } = new List<Season>();

    public virtual ICollection<TeamSeasonLeague> TeamSeasonLeagues { get; set; } = new List<TeamSeasonLeague>();

    public virtual ICollection<TeamStatistic> TeamStatistics { get; set; } = new List<TeamStatistic>();
}
