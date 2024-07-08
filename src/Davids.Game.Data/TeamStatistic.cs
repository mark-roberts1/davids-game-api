namespace Davids.Game.Data;

public partial class TeamStatistic
{
    public long Id { get; set; }

    public long TeamId { get; set; }

    public int LeagueId { get; set; }

    public short Year { get; set; }

    public DateOnly Week { get; set; }

    public long StatisticId { get; set; }

    public virtual League League { get; set; } = null!;

    public virtual Season Season { get; set; } = null!;

    public virtual Statistic Statistic { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;
}
