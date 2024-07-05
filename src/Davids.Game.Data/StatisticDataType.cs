namespace Davids.Game.Data;

public partial class StatisticDataType
{
    public short Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Statistic> Statistics { get; set; } = new List<Statistic>();
}
