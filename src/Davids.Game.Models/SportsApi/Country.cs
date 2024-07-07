using System.Text.Json.Serialization;

namespace Davids.Game.Models.SportsApi;
public class Country
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("code")]
    public string Code { get; set; } = null!;

    [JsonPropertyName("flag")]
    public string Flag { get; set; } = null!;
}
