namespace Davids.Game.Models.Countries;

public class CountryResponse
{
    public short Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? FlagLink { get; set; }
}
