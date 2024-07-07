using System.Text.Json.Serialization;

namespace Davids.Game.Models.SportsApi;

public class LeagueResponseItem
{
    [JsonPropertyName("league")]
    public League League { get; set; } = null!;

    [JsonPropertyName("country")]
    public Country Country { get; set; } = null!;

    [JsonPropertyName("seasons")]
    public IEnumerable<Season> Seasons { get; set; } = null!;
}
