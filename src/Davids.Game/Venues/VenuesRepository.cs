using AutoMapper;
using Davids.Game.Data;
using Davids.Game.DependencyInjection;
using Davids.Game.Extensions;
using Davids.Game.Models;
using Davids.Game.Models.Venues;
using Microsoft.EntityFrameworkCore;

namespace Davids.Game.Venues;

[Service<IVenuesRepository>]
internal class VenuesRepository(IDbContextFactory<DavidsGameContext> contextFactory, IMapper mapper) : IVenuesRepository
{
    public async Task<short> CreateSurfaceTypeAsync(string name, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var type = new SurfaceType() { Name = name };
        context.SurfaceTypes.Add(type);

        await context.SaveChangesAsync(cancellationToken);

        return type.Id;
    }

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

    public async Task<IEnumerable<EnumerationResponse>> GetSurfaceTypesAsync(CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        return await context
            .SurfaceTypes
            .Select(t => new EnumerationResponse { Id = t.Id, Name = t.Name })
            .ToListAsync(cancellationToken);
    }

    public async Task<VenueResponse?> GetVenueAsync(long venueId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var venue = await context.Venues.SingleOrDefaultAsync(v => v.Id == venueId, cancellationToken);

        if (venue == null) return null;

        return mapper.Map<VenueResponse?>(venue);
    }

    public async Task<VenueResponse?> GetVenueBySourceIdAsync(long sourceId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var venue = await context.Venues.SingleOrDefaultAsync(v => v.SourceId == sourceId, cancellationToken);

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
