namespace Davids.Game.Data;

public partial class ListEntry
{
    public long Id { get; set; }

    public long ListId { get; set; }

    public long TeamId { get; set; }

    public virtual List List { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;

    public virtual ICollection<Statistic> Statistics { get; set; } = new List<Statistic>();
}
