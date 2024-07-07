using Davids.Game.Models;
using Davids.Game.Models.Leagues;
using Davids.Game.Models.Teams;

namespace Davids.Game.Leagues;

public interface ILeaguesRepository
{
    Task<IEnumerable<EnumerationResponse>> GetLeagueTypesAsync(CancellationToken cancellationToken);
    Task<short> AddLeagueTypeAsync(string name, CancellationToken cancellationToken);
    Task<PageResponse<LeagueResponse>> SearchLeaguesAsync(LeagueSearchRequest request, CancellationToken cancellationToken);
    Task<LeagueResponse?> GetLeagueAsync(int leagueId, CancellationToken cancellationToken);
    Task<int> CreateLeagueAsync(LeagueWriteRequest request, CancellationToken cancellationToken);
    Task<bool> UpdateLeagueAsync(int leagueId, LeagueWriteRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteLeagueAsync(int leagueId, CancellationToken cancellationToken);
    Task<PageResponse<TeamResponse>> GetTeamsAsync(int leagueId, string? season, int? pageNumber, int? pageSize, CancellationToken cancellationToken);

}
