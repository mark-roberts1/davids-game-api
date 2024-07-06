using Davids.Game.Models.Countries;

namespace Davids.Game.Models.Leagues;

public class LeagueResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public short LeagueTypeId { get; set; }

    public string? LogoLink { get; set; }

    public short? CountryId { get; set; }

    public long? SourceId { get; set; }

    public CountryResponse? Country { get; set; }
}
