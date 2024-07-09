using System.Text.Json.Serialization;

namespace Davids.Game.Models.SportsApi;

public class Fixtures
{

    [JsonPropertyName("played")]
    public Record Played { get; set; } = null!;

    [JsonPropertyName("wins")]
    public Record Wins { get; set; } = null!;

    [JsonPropertyName("draws")]
    public Record Draws { get; set; } = null!;

    [JsonPropertyName("loses")]
    public Record Losses { get; set; } = null!;
}
