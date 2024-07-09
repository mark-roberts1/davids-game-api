using System.Text.Json.Serialization;

namespace Davids.Game.Models.SportsApi;

public class StatisticsReponse
{
    [JsonPropertyName("response")]
    public Statistic Response { get; set; } = null!;
}
