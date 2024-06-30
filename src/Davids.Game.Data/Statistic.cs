using System;
using System.Collections.Generic;

namespace Davids.Game.Data;

public partial class Statistic
{
    public long Id { get; set; }

    public short Type { get; set; }

    public byte[] Value { get; set; } = null!;

    public virtual ICollection<TeamSeasonStatistic> TeamSeasonStatistics { get; set; } = new List<TeamSeasonStatistic>();

    public virtual ICollection<ListEntry> ListEntries { get; set; } = new List<ListEntry>();
}
