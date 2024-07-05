namespace Davids.Game.Models.Statistics;
public class StatisticWriteRequest
{
    public short StatisticTypeId { get; set; }

    public string Value { get; set; } = null!;

    public short StatisticDataTypeId { get; set; }
}
