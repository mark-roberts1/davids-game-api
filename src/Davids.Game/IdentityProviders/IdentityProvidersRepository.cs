using AutoMapper;
using Davids.Game.Data;
using Davids.Game.DependencyInjection;
using Davids.Game.Models.IdentityProviders;
using Microsoft.EntityFrameworkCore;

namespace Davids.Game.IdentityProviders;

[Service<IIdentityProvidersRepository>]
internal class IdentityProvidersRepository(IDbContextFactory<DavidsGameContext> contextFactory, IMapper mapper) : IIdentityProvidersRepository
{
    public async Task<IEnumerable<IdentityProviderResponse>> GetIdentityProvidersAsync(CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        return await context
            .IdentityProviders
            .Select(i => mapper.Map<IdentityProviderResponse>(i))
            .ToArrayAsync(cancellationToken);
    }
}
