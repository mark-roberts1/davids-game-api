﻿namespace Davids.Game.Models.Venues;

public class VenueWriteRequest
{
    public string Name { get; set; } = null!;

    public int? Capacity { get; set; }

    public short? SurfaceTypeId { get; set; }

    public string? ImageLink { get; set; }

    public long? SourceId { get; set; }
}
