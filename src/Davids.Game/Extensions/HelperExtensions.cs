namespace Davids.Game;

public static class HelperExtensions
{
    public static DateOnly GetDateOnly(this DateTime date)
    {
        return new(date.Year, date.Month, date.Day);
    }
}
