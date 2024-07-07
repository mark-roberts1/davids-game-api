using System.Text.Json.Serialization;

namespace Davids.Game.Models.SportsApi;

public class LeagueResponse
{
    [JsonPropertyName("response")]
    public IEnumerable<LeagueResponseItem> Response { get; set; } = null!;
}
