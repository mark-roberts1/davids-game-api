using Davids.Game.Models.Leagues;
using Davids.Game.SportsApi;
using System.Threading.Channels;

namespace Davids.Game.Api.HostedServices;

public class TeamStatisticsDataLoader(
    SportsApiHttpClient sportsApi,
    IRepository repository,
    IConfiguration configuration,
    ChannelReader<LeagueSeasonStatsRequest> queue) : BackgroundService
{
    private IEnumerable<short> GetSeasonYears()
    {
        var current = DateTime.Now.Year;

        for (var year = current - 1; year <= current + 1; year++)
            yield return (short)year;
    }

    private IEnumerable<DateTime> GetAllOccurrences(DayOfWeek dayOfWeek, DateTime min, DateTime max)
    {
        while (min.DayOfWeek != dayOfWeek) min = min.AddDays(1);
        while (max.DayOfWeek != dayOfWeek) max = max.AddDays(1);

        var items = new List<DateTime>();

        while (min <= max)
        {
            items.Add(min);

            min = min.AddDays(7);
        }

        return items;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        while (!stoppingToken.IsCancellationRequested)
        {
            var request = await queue.ReadAsync(stoppingToken);

            var season = await repository.Seasons.GetSeasonAsync(request.LeagueId, request.Year, stoppingToken);

            if (season == null) continue;

            var end = season.EndDate.ToDateTime(default);

            if (end > DateTime.Now) end = DateTime.Now;

            var dates = GetAllOccurrences(DayOfWeek.Tuesday, season.StartDate.ToDateTime(default), end);
            var teams = await repository.Leagues.GetTeamsAsync(request.LeagueId, request.Year.ToString(), null, null, stoppingToken);

            foreach (var team in teams.Page)
            {
                foreach (var date in dates)
                {

                }
            }
        }
    }
}
