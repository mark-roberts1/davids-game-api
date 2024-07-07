using System.Text.Json.Serialization;

namespace Davids.Game.Models.SportsApi;

public class Season
{
    [JsonPropertyName("year")]
    public short Year { get; set; }

    [JsonPropertyName("start")]
    public DateTime Start { get; set; }

    [JsonPropertyName("end")]
    public DateTime End { get; set; }

    [JsonPropertyName("current")]
    public bool Current { get; set; }

    [JsonPropertyName("coverage")]
    public Coverage Coverage { get; set; } = null!;
}
