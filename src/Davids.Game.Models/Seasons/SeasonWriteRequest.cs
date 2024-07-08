namespace Davids.Game.Models.Seasons;

public class SeasonWriteRequest
{
    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool Current { get; set; }
}
