using System.Text.Json.Serialization;

namespace Davids.Game.Models.Users;
public class DiscordUser
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("username")]
    public string Username { get; set; } = null!;

    [JsonPropertyName("avatar")]
    public string Avatar { get; set; } = null!;
}
