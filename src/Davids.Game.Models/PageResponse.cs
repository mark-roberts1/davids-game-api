namespace Davids.Game.Models;

public record PageResponse<T>(int Total, IEnumerable<T> Page);

public static class PageResponseExtensions
{
    public static PageResponse<T> ToPageResponse<T>(this IEnumerable<T> response, int totalCount)
    {
        return new PageResponse<T>(totalCount, response);
    }
}