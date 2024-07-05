using AutoMapper;
using Davids.Game.Data;
using Davids.Game.DependencyInjection;
using Davids.Game.Extensions;
using Davids.Game.Models.Venues;
using Microsoft.EntityFrameworkCore;

namespace Davids.Game.Venues;

[Service<IVenuesRepository>]
internal class VenuesRepository(IDbContextFactory<DavidsGameContext> contextFactory, IMapper mapper) : IVenuesRepository
{
    public async Task<long> CreateVenueAsync(VenueWriteRequest request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var venue = mapper.Map<Venue>(request);

        context.Venues.Add(venue);

        await context.SaveChangesAsync(cancellationToken);

        return venue.Id;
    }

    public async Task<bool> DeleteVenueAsync(long venueId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var venue = context.Venues.FindLocalOrAttach(v => v.Id == venueId, new() { Id = venueId });

        try
        {
            context.Venues.Remove(venue);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public async Task<VenueResponse?> GetVenueAsync(long venueId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var venue = await context.Venues.SingleOrDefaultAsync(v => v.Id == venueId, cancellationToken);

        if (venue == null) return null;

        return mapper.Map<VenueResponse?>(venue);
    }

    public async Task<bool> UpdateVenueAsync(long venueId, VenueWriteRequest request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var venue = await context.Venues.SingleOrDefaultAsync(v => v.Id == venueId, cancellationToken);

        if (venue == null) return false;

        context.Entry(venue).CurrentValues.SetValues(request);

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
