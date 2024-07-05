using Davids.Game.Models.Statistics;

namespace Davids.Game.Models.Lists;
public class ListEntryWriteRequest
{
    public long TeamId { get; set; }

    public IEnumerable<StatisticWriteRequest> Statistics { get; set; } = [];
}
