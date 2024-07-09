using System.Text.Json.Serialization;

namespace Davids.Game.Models.SportsApi;

public class Record
{
    [JsonPropertyName("home")]
    public short Home { get; set; }

    [JsonPropertyName("away")]
    public short Away { get; set; }

    [JsonPropertyName("total")]
    public short Total { get; set; }
}
