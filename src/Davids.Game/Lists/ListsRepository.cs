using AutoMapper;
using Davids.Game.Data;
using Davids.Game.DependencyInjection;
using Davids.Game.Extensions;
using Davids.Game.Models.Lists;
using Microsoft.EntityFrameworkCore;

namespace Davids.Game.Lists;

[Service<IListsRepository>]
internal class ListsRepository(IDbContextFactory<DavidsGameContext> contextFactory, IMapper mapper) : IListsRepository
{
    public async Task<long> CreateListAsync(ListWriteRequest request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var entity = mapper.Map<List>(request);

        context.Lists.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task<bool> DeleteListAsync(long listId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var list = context.Lists.FindLocalOrAttach(l => l.Id == listId, new() { Id = listId });

        try
        {
            context.Lists.Remove(list);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public async Task<ListResponse?> GetListAsync(long listId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var list = await context
            .Lists
            .Include(l => l.PreviousList)
            .Include(l => l.ListEntries)
            .SingleOrDefaultAsync(l => l.Id == listId, cancellationToken);

        if (list == null) return null;

        return mapper.Map<ListResponse>(list);
    }

    public async Task<bool> UpdateListAsync(long listId, ListWriteRequest request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var list = await context.Lists.SingleOrDefaultAsync(l => l.Id == listId, cancellationToken);

        if (list == null) return false;

        var entries = await context.ListEntries.Where(e => e.ListId == listId).ToListAsync(cancellationToken);
        
        if (entries.Any())
        {
            context.ListEntries.RemoveRange(entries);

            await context.SaveChangesAsync(cancellationToken);
        }

        context.Entry(list).Property(l => l.PreviousListId).CurrentValue = request.PreviousListId;
        context.Entry(list).Property(l => l.PreviousListId).CurrentValue = request.PreviousListId;
        context.Entry(list).Property(l => l.UserPoolId).CurrentValue = request.UserPoolId;

        entries = request.Entries.Select(e => mapper.Map<ListEntry>(e)).ToList();

        if (entries.Any())
        {
            foreach (var entry in entries) entry.ListId = listId;

            context.ListEntries.AddRange(entries);
        }

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
