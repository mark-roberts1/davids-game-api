using Davids.Game.Models;
using Davids.Game.Models.Venues;

namespace Davids.Game.Venues;

public interface IVenuesRepository
{
    Task<IEnumerable<EnumerationResponse>> GetSurfaceTypesAsync(CancellationToken cancellationToken);
    Task<short> CreateSurfaceTypeAsync(string name, CancellationToken cancellationToken);
    Task<VenueResponse?> GetVenueAsync(long venueId, CancellationToken cancellationToken);
    Task<VenueResponse?> GetVenueBySourceIdAsync(long sourceId, CancellationToken cancellationToken);
    Task<long> CreateVenueAsync(VenueWriteRequest request, CancellationToken cancellationToken);
    Task<bool> UpdateVenueAsync(long venueId, VenueWriteRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteVenueAsync(long venueId, CancellationToken cancellationToken);
}
