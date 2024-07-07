using System.Text.Json.Serialization;

namespace Davids.Game.Models.SportsApi;

public class Coverage
{
    [JsonPropertyName("fixtures")]
    public CoverageFixtures Fixtures { get; set; } = null!;

    [JsonPropertyName("standings")]
    public bool Standings { get; set; }

    [JsonPropertyName("players")]
    public bool Players { get; set; }

    [JsonPropertyName("top_scorers")]
    public bool TopScorers { get; set; }

    [JsonPropertyName("top_assists")]
    public bool TopAssists { get; set; }

    [JsonPropertyName("top_cards")]
    public bool TopCards { get; set; }

    [JsonPropertyName("injuries")]
    public bool Injuries { get; set; }

    [JsonPropertyName("predictions")]
    public bool Predictions { get; set; }

    [JsonPropertyName("odds")]
    public bool Odds { get; set; }
}
