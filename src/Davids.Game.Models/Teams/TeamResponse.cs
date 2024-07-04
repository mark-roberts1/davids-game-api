﻿using Davids.Game.Models.Countries;

namespace Davids.Game.Models.Teams;
public class TeamResponse
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? Code { get; set; }

    public string? LogoLink { get; set; }

    public short? CountryId { get; set; }

    public long? SourceId { get; set; }

    public CountryResponse? Country { get; set; }
}
