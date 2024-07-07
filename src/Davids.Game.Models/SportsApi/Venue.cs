using System.Text.Json.Serialization;

namespace Davids.Game.Models.SportsApi;

public class Venue
{
    [JsonPropertyName("id")]
    public long? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("capacity")]
    public int? Capacity { get; set; }

    [JsonPropertyName("surface")]
    public string? Surface { get; set; } = null!;

    [JsonPropertyName("image")]
    public string Image { get; set; } = null!;
}
