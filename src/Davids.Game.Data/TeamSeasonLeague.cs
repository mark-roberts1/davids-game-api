using System;
using System.Collections.Generic;

namespace Davids.Game.Data;

public partial class TeamSeasonLeague
{
    public int LeagueId { get; set; }

    public long TeamId { get; set; }

    public string Season { get; set; } = null!;

    public virtual League League { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;
}
