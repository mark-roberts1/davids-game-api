using AutoMapper;
using Davids.Game.Data;
using Davids.Game.DependencyInjection;
using Davids.Game.Models.UserPools;
using Davids.Game.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Davids.Game.Users;

[Service<IUsersRepository>]
internal class UsersRepository(IDbContextFactory<DavidsGameContext> contextFactory, IMapper mapper) : IUsersRepository
{
    public async Task<long> CreateUserAsync(UserWriteRequest request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var user = mapper.Map<User>(context);

        await context.Users.AddAsync(user, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }

    public async Task<IEnumerable<UserPoolResponse>> GetPoolsAsync(long userId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        return await context
            .UserPools
                .Include(up => up.Pool)
            .Where(up => up.UserId == userId)
            .Select(up => mapper.Map<UserPoolResponse>(up))
            .ToListAsync(cancellationToken);
    }

    public async Task<UserResponse?> GetUserAsync(long userId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var user = await context.Users.SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null) return null;

        return mapper.Map<UserResponse>(user);
    }

    public async Task<UserResponse?> GetUserAsync(short identityProviderId, string id, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var user = await context
            .Users
            .SingleOrDefaultAsync(u => u.ExternalId == id && u.IdentityProviderId == identityProviderId, cancellationToken);

        if (user == null) return null;

        return mapper.Map<UserResponse>(user);
    }

    public async Task<bool> UpdateUserAsync(long userId, UserWriteRequest request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var user = await context.Users.SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null) return false;

        context.Entry(user).CurrentValues.SetValues(request);

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
