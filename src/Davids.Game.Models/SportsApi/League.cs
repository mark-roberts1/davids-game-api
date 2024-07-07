using System.Text.Json.Serialization;

namespace Davids.Game.Models.SportsApi;
public class League
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    [JsonPropertyName("logo")]
    public string Logo { get; set; } = null!;
}
