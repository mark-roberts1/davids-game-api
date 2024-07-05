namespace Davids.Game.Models.Statistics;
public class StatisticResponse
{
    public long Id { get; set; }

    public short StatisticTypeId { get; set; }

    public string Value { get; set; } = null!;

    public short StatisticDataTypeId { get; set; }
}
