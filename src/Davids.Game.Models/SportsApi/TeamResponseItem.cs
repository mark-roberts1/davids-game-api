using System.Text.Json.Serialization;

namespace Davids.Game.Models.SportsApi;

public class TeamResponseItem
{
    [JsonPropertyName("team")]
    public Team Team { get; set; } = null!;

    [JsonPropertyName("venue")]
    public Venue Venue { get; set; } = null!;
}
