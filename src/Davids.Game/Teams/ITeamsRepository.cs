using Davids.Game.Models.Leagues;
using Davids.Game.Models.Statistics;
using Davids.Game.Models.Teams;
using Davids.Game.Models.Venues;

namespace Davids.Game.Teams;

public interface ITeamsRepository
{
    Task<TeamResponse?> GetTeamAsync(long teamId, CancellationToken cancellationToken);
    Task<long> CreateTeamAsync(TeamWriteRequest request, CancellationToken cancellationToken);
    Task<bool> UpdateTeamAsync(long teamId, TeamWriteRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteTeamAsync(long teamId, CancellationToken cancellationToken);

    Task<IEnumerable<StatisticResponse>> GetStatisticsAsync(long teamId, string season, CancellationToken cancellationToken);
    Task<IEnumerable<LeagueSeasonResponse>> GetLeaguesAsync(long teamId, CancellationToken cancellationToken);
    Task<IEnumerable<VenueResponse>> GetVenuesAsync(long teamId, CancellationToken cancellationToken);
    Task<bool> AddLeagueAsync(long teamId, int leagueId, string season, CancellationToken cancellationToken);
    Task SaveStatisticsAsync(long teamId, string season, IEnumerable<StatisticResponse> statistics, CancellationToken cancellationToken);
}
