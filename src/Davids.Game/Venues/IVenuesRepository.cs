using Davids.Game.Models.Venues;

namespace Davids.Game.Venues;

public interface IVenuesRepository
{
    Task<VenueResponse?> GetVenueAsync(long venueId, CancellationToken cancellationToken);
    Task<long> CreateVenueAsync(VenueWriteRequest request, CancellationToken cancellationToken);
    Task<bool> UpdateVenueAsync(long venueId, VenueWriteRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteVenueAsync(long venueId, CancellationToken cancellationToken);
}
