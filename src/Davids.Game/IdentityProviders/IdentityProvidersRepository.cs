using AutoMapper;
using Davids.Game.Data;
using Davids.Game.DependencyInjection;
using Davids.Game.Models;
using Microsoft.EntityFrameworkCore;

namespace Davids.Game.IdentityProviders;

[Service<IIdentityProvidersRepository>]
internal class IdentityProvidersRepository(IDbContextFactory<DavidsGameContext> contextFactory, IMapper mapper) : IIdentityProvidersRepository
{
    public async Task<IEnumerable<EnumerationResponse>> GetIdentityProvidersAsync(CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        return await context
            .IdentityProviders
            .Select(i => mapper.Map<EnumerationResponse>(i))
            .ToArrayAsync(cancellationToken);
    }
}
