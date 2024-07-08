using Davids.Game.Models.Leagues;
using Davids.Game.Models.Seasons;

namespace Davids.Game.Seasons;

public interface ISeasonsRepository
{
    Task<SeasonResponse?> GetSeasonAsync(int leagueId, short year, CancellationToken cancellationToken);
    Task<IEnumerable<SeasonResponse>> GetSeasonsAsync(int leagueId, CancellationToken cancellationToken);
    Task CreateSeasonAsync(int leagueId, short year, SeasonWriteRequest request, CancellationToken cancellationToken);
    Task<bool> UpdateSeasonAsync(int leagueId, short year, SeasonWriteRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteSeasonAsync(int leagueId, short year, CancellationToken cancellationToken);

    Task<IEnumerable<LeagueResponse>> GetLeaguesAsync(short year, CancellationToken cancellationToken);
}
