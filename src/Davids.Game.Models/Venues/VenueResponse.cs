namespace Davids.Game.Models.Venues;

public class VenueResponse
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public int? Capacity { get; set; }

    public SurfaceType? Surface { get; set; }

    public string? ImageLink { get; set; }

    public long? SourceId { get; set; }
}
