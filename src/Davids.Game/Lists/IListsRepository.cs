using Davids.Game.Models.Lists;

namespace Davids.Game.Lists;
public interface IListsRepository
{
    Task<ListResponse?> GetListAsync(long listId, CancellationToken cancellationToken);
    Task<long> CreateListAsync(ListWriteRequest request, CancellationToken cancellationToken);
    Task<bool> UpdateListAsync(long listId, ListWriteRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteListAsync(long listId, CancellationToken cancellationToken);
}
