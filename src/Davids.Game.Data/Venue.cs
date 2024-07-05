namespace Davids.Game.Data;

public partial class Venue
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public int? Capacity { get; set; }

    public short? Surface { get; set; }

    public string? ImageLink { get; set; }

    public long? SourceId { get; set; }

    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
}
