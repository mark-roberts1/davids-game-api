namespace Davids.Game.Models.Seasons;

public class SeasonResponse
{
    public int LeagueId { get; set; }

    public short Year { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool Current { get; set; }
}
