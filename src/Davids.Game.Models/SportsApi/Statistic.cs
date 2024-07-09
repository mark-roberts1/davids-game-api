using System.Text.Json.Serialization;

namespace Davids.Game.Models.SportsApi;

public class Statistic
{
    [JsonPropertyName("league")]
    public League League { get; set; } = null!;

    [JsonPropertyName("team")]
    public Team Team { get; set; } = null!;

    [JsonPropertyName("fixtures")]
    public Fixtures Fixtures { get; set; } = null!;

    [JsonPropertyName("form")]
    public string Form {  get; set; } = null!;

    [JsonPropertyName("goals")]
    public GoalCountsWrapper Goals { get; set; } = null!;
}
