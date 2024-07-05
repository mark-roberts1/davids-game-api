using Davids.Game.Models.Statistics;

namespace Davids.Game.Models.Lists;
public class ListEntryResponse
{
    public long Id { get; set; }

    public long TeamId { get; set; }

    public IEnumerable<StatisticResponse> Statistics { get; set; } = [];
}
