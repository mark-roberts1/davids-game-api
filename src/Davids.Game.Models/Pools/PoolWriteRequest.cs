namespace Davids.Game.Models.Pools;

public class PoolWriteRequest
{
    public string Name { get; set; } = null!;

    public int? LeagueId { get; set; }

    public string Season { get; set; } = null!;

    public string? DiscordServerId { get; set; }

    public string JoinCode { get; set; } = null!;
}
