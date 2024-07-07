using System.Text.Json.Serialization;

namespace Davids.Game.Models.SportsApi;

public class TeamResponse
{
    [JsonPropertyName("response")]
    public IEnumerable<TeamResponseItem> Response { get; set; } = null!;
}
