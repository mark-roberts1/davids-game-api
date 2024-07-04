namespace Davids.Game.Models.Countries;

public class CountryWriteRequest
{
    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? FlagLink { get; set; }
}
