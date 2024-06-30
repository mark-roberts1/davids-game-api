using System;
using System.Collections.Generic;

namespace Davids.Game.Data;

public partial class UserPool
{
    public long Id { get; set; }

    public long PoolId { get; set; }

    public long UserId { get; set; }

    public long Attributes { get; set; }

    public virtual ICollection<List> Lists { get; set; } = new List<List>();

    public virtual Pool Pool { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
