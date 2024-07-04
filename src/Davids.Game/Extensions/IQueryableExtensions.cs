using Davids.Game.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Davids.Game.Extensions;

public static class IQueryableExtensions
{
    public static async Task<PageResponse<T>> ToPageResponseAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var count = await query.CountAsync(cancellationToken);

        query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        return (await query.ToArrayAsync(cancellationToken)).ToPageResponse(count);
    }
}
