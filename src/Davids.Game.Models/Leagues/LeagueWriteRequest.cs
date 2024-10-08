﻿namespace Davids.Game.Models.Leagues;

public class LeagueWriteRequest
{
    public string Name { get; set; } = null!;

    public short LeagueTypeId { get; set; }

    public string? LogoLink { get; set; }

    public short? CountryId { get; set; }

    public long? SourceId { get; set; }
}
