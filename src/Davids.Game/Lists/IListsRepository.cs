using Davids.Game.Models.Lists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Davids.Game.Lists;
public interface IListsRepository
{
    Task<ListResponse?> GetListAsync(long listId, CancellationToken cancellationToken);
    Task<>
}
