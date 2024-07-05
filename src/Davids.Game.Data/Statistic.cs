namespace Davids.Game.Data;

public partial class Statistic
{
    public long Id { get; set; }

    public short StatisticTypeId { get; set; }

    public string Value { get; set; } = null!;

    public short StatisticDataTypeId { get; set; }

    public virtual StatisticDataType StatisticDataType { get; set; } = null!;

    public virtual StatisticType StatisticType { get; set; } = null!;

    public virtual ICollection<TeamSeasonStatistic> TeamSeasonStatistics { get; set; } = new List<TeamSeasonStatistic>();

    public virtual ICollection<ListEntry> ListEntries { get; set; } = new List<ListEntry>();
}
