namespace Davids.Game.Models.Statistics;
public class StatisticResponse
{
    public long Id { get; set; }

    public StatisticType Type { get; set; }

    public string Value { get; set; } = null!;
}
