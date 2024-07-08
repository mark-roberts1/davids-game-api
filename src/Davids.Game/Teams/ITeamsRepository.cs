using Davids.Game.Models.Leagues;
using Davids.Game.Models.Teams;
using Davids.Game.Models.Venues;

namespace Davids.Game.Teams;

public interface ITeamsRepository
{
    Task<TeamResponse?> GetTeamAsync(long teamId, CancellationToken cancellationToken);
    Task<TeamResponse?> GetTeamBySourceIdAsync(long sourceId, CancellationToken cancellationToken);
    Task<long> CreateTeamAsync(TeamWriteRequest request, CancellationToken cancellationToken);
    Task<bool> UpdateTeamAsync(long teamId, TeamWriteRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteTeamAsync(long teamId, CancellationToken cancellationToken);

    Task<IEnumerable<LeagueSeasonResponse>> GetLeaguesAsync(long teamId, CancellationToken cancellationToken);
    Task<IEnumerable<VenueResponse>> GetVenuesAsync(long teamId, CancellationToken cancellationToken);
    Task AddVenueAsync(long teamId, long venueId, CancellationToken cancellationToken);
    Task<bool> AddLeagueAsync(long teamId, int leagueId, string season, CancellationToken cancellationToken);
}
