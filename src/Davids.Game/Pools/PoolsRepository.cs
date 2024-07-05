using AutoMapper;
using Davids.Game.Data;
using Davids.Game.DependencyInjection;
using Davids.Game.Extensions;
using Davids.Game.Models.Lists;
using Davids.Game.Models.Pools;
using Davids.Game.Models.UserPools;
using Davids.Game.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Davids.Game.Pools;

[Service<IPoolsRepository>]
internal class PoolsRepository(IDbContextFactory<DavidsGameContext> contextFactory, IMapper mapper) : IPoolsRepository
{
    public async Task<long> AddUserAsync(long poolId, UserPoolWriteRequest request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        UserPool up = new()
        {
            Attributes = (long)request.Attributes,
            PoolId = poolId,
            UserId = request.UserId,
        };

        context.UserPools.Add(up);

        await context.SaveChangesAsync(cancellationToken);

        return up.Id;
    }

    public async Task<long> CreatePoolAsync(PoolWriteRequest request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var entity = mapper.Map<Pool>(request);

        context.Pools.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task<bool> DeletePoolAsync(long poolId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var entity = context.Pools.FindLocalOrAttach(p => p.Id == poolId, new() { Id = poolId });

        try
        {
            context.Pools.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public async Task<IEnumerable<ListResponse>> GetListsAsync(long poolId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        return await context
            .Lists
                .Include(l => l.ListEntries)
                .Include(l => l.UserPool)
            .Where(l => l.UserPool.PoolId == poolId)
            .Select(l => mapper.Map<ListResponse>(l))
            .ToListAsync(cancellationToken);
    }

    public async Task<PoolResponse?> GetPoolAsync(long poolId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var pool = await context.Pools.SingleOrDefaultAsync(p => p.Id == poolId, cancellationToken);

        if (pool == null) return null;

        return mapper.Map<PoolResponse>(pool);
    }

    public async Task<IEnumerable<PoolUserResponse>> GetUsersAsync(long poolId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        return await context
            .UserPools
                .Include(up => up.User)
                .Include(up => up.Pool)
            .Where(up => up.PoolId == poolId)
            .Select(up => mapper.Map<PoolUserResponse>(up))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> RemoveUserAsync(long poolId, long userId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var entity = context.UserPools.FindLocalOrAttach(up => up.PoolId == poolId && up.UserId == userId, new() { UserId = userId, PoolId = poolId });

        try
        {
            context.UserPools.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public async Task<bool> UpdatePoolAsync(long poolId, PoolWriteRequest request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var pool = await context.Pools.SingleOrDefaultAsync(p => p.Id == poolId, cancellationToken);

        if (pool == null) return false;

        context.Entry(pool).CurrentValues.SetValues(request);
        
        return true;
    }
}
