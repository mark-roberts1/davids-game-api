using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Davids.Game.Models;

public record CollectionResponse<T>(int Count, IEnumerable<T> Items);

public static class CollectionResponseExtensions
{
    public static CollectionResponse<T> ToCollectionResponse<T>(this IEnumerable<T> items)
    {
        return new CollectionResponse<T>(items.Count(), items);
    }
}
