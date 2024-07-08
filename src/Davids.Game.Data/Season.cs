namespace Davids.Game.Data;

public partial class Season
{
    public int LeagueId { get; set; }

    public short Year { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool Current { get; set; }

    public virtual League League { get; set; } = null!;

    public virtual ICollection<TeamStatistic> TeamStatistics { get; set; } = new List<TeamStatistic>();
}
