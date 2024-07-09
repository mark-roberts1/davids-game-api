using System.Text.Json.Serialization;

namespace Davids.Game.Models.SportsApi;

public class GoalCounts
{
    [JsonPropertyName("total")]
    public Record Total { get; set; } = null!;
}

public class GoalCountsWrapper
{
    [JsonPropertyName("for")]
    public GoalCounts For { get; set; } = null!;

    [JsonPropertyName("against")]
    public GoalCounts Against { get; set; } = null!;
}