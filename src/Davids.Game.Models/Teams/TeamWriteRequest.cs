namespace Davids.Game.Models.Teams;

public class TeamWriteRequest
{
    public string? Name { get; set; }

    public string? Code { get; set; }

    public string? LogoLink { get; set; }

    public short? CountryId { get; set; }

    public long? SourceId { get; set; }
}
