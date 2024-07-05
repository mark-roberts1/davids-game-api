using Davids.Game.Models;
using Microsoft.EntityFrameworkCore;

namespace Davids.Game.Extensions;

public static class IQueryableExtensions
{
    public static async Task<PageResponse<T>> ToPageResponseAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var count = await query.CountAsync(cancellationToken);

        query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        return (await query.ToArrayAsync(cancellationToken)).ToPageResponse(count);
    }

    public static T FindLocalOrAttach<T>(this DbSet<T> entities, Func<T, bool> predicate, T defaultEntity) where T : class
    {
        var entity = entities.Local.SingleOrDefault(predicate);

        if (entity == null)
        {
            entity = defaultEntity;

            entities.Attach(entity);
        }

        return entity;
    }
}
