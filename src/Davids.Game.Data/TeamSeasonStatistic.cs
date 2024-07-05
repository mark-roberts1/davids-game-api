namespace Davids.Game.Data;

public partial class TeamSeasonStatistic
{
    public long TeamId { get; set; }

    public string Season { get; set; } = null!;

    public long StatisticId { get; set; }

    public virtual Statistic Statistic { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;
}
