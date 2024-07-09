using Davids.Game.Models.Leagues;
using Davids.Game.Models.SportsApi;
using Davids.Game.Models.Statistics;
using Davids.Game.SportsApi;
using System.Reflection;
using System.Threading.Channels;

namespace Davids.Game.Api.HostedServices;

public class TeamStatisticsDataLoader(
    SportsApiHttpClient sportsApi,
    IRepository repository,
    ChannelReader<LeagueSeasonStatsRequest> queue) : BackgroundService
{
    private const short IntegerDataType = 2;

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var types = (await repository.Statistics.GetStatisticTypesAsync(cancellationToken)).ToList();

        var candidates = new string[]
        {
            "Played - Home",
            "Played - Away",
            "Played - Total",
            "Wins - Home",
            "Wins - Away",
            "Wins - Total",
            "Draws - Home",
            "Draws - Away",
            "Draws - Total",
            "Losses - Home",
            "Losses - Away",
            "Losses - Total",
            "Goals - For - Home",
            "Goals - For - Away",
            "Goals - For - Total",
            "Goals - Against - Home",
            "Goals - Against - Away",
            "Goals - Against - Total",
        };

        foreach (var name in candidates) 
        {
            if (!types.Any(t => t.Name == name))
            {
                var id = await repository.Statistics.CreateStatisticTypeAsync(name, cancellationToken);

                types.Add(new() { Id = id, Name = name });
            }
        }

        await base.StartAsync(cancellationToken);
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

            var statisticTypes = await repository.Statistics.GetStatisticTypesAsync(stoppingToken);
            var season = await repository.Seasons.GetSeasonAsync(request.LeagueId, request.Year, stoppingToken);
            var league = await repository.Leagues.GetLeagueAsync(request.LeagueId, stoppingToken);

            if (season == null) continue;
            if (league == null) continue;

            var end = season.EndDate.ToDateTime(default);

            if (end > DateTime.Now) end = DateTime.Now;

            var dates = GetAllOccurrences(DayOfWeek.Tuesday, season.StartDate.ToDateTime(default), end);
            var teams = await repository.Leagues.GetTeamsAsync(request.LeagueId, request.Year.ToString(), null, null, stoppingToken);

            foreach (var team in teams.Page)
            {
                foreach (var date in dates)
                {
                    var statisticResponse = await sportsApi.GetStatisticsAsync(
                        season.Year,
                        league.SourceId.GetValueOrDefault(),
                        team.SourceId.GetValueOrDefault(),
                        date,
                        stoppingToken);

                    var fixtures = statisticResponse.Response.Fixtures;
                    var goalStats = statisticResponse.Response.Goals;

                    var stats = new StatisticWriteRequest[]
                    {
                        new()
                        {
                            StatisticDataTypeId = IntegerDataType,
                            StatisticTypeId = statisticTypes.Single(t => t.Name == "Played - Home").Id,
                            Value = fixtures.Played.Home.ToString()
                        },
                        new()
                        {
                            StatisticDataTypeId = IntegerDataType,
                            StatisticTypeId = statisticTypes.Single(t => t.Name == "Played - Away").Id,
                            Value = fixtures.Played.Away.ToString()
                        },
                        new()
                        {
                            StatisticDataTypeId = IntegerDataType,
                            StatisticTypeId = statisticTypes.Single(t => t.Name == "Played - Total").Id,
                            Value = fixtures.Played.Total.ToString()
                        },
                        new()
                        {
                            StatisticDataTypeId = IntegerDataType,
                            StatisticTypeId = statisticTypes.Single(t => t.Name == "Wins - Home").Id,
                            Value = fixtures.Wins.Home.ToString()
                        },
                        new()
                        {
                            StatisticDataTypeId = IntegerDataType,
                            StatisticTypeId = statisticTypes.Single(t => t.Name == "Wins - Away").Id,
                            Value = fixtures.Wins.Away.ToString()
                        },
                        new()
                        {
                            StatisticDataTypeId = IntegerDataType,
                            StatisticTypeId = statisticTypes.Single(t => t.Name == "Wins - Total").Id,
                            Value = fixtures.Wins.Total.ToString()
                        },
                        new()
                        {
                            StatisticDataTypeId = IntegerDataType,
                            StatisticTypeId = statisticTypes.Single(t => t.Name == "Draws - Home").Id,
                            Value = fixtures.Draws.Home.ToString()
                        },
                        new()
                        {
                            StatisticDataTypeId = IntegerDataType,
                            StatisticTypeId = statisticTypes.Single(t => t.Name == "Draws - Away").Id,
                            Value = fixtures.Draws.Away.ToString()
                        },
                        new()
                        {
                            StatisticDataTypeId = IntegerDataType,
                            StatisticTypeId = statisticTypes.Single(t => t.Name == "Draws - Total").Id,
                            Value = fixtures.Draws.Total.ToString()
                        },
                        new()
                        {
                            StatisticDataTypeId = IntegerDataType,
                            StatisticTypeId = statisticTypes.Single(t => t.Name == "Losses - Home").Id,
                            Value = fixtures.Losses.Home.ToString()
                        },
                        new()
                        {
                            StatisticDataTypeId = IntegerDataType,
                            StatisticTypeId = statisticTypes.Single(t => t.Name == "Losses - Away").Id,
                            Value = fixtures.Losses.Away.ToString()
                        },
                        new()
                        {
                            StatisticDataTypeId = IntegerDataType,
                            StatisticTypeId = statisticTypes.Single(t => t.Name == "Losses - Total").Id,
                            Value = fixtures.Losses.Total.ToString()
                        },
                        new()
                        {
                            StatisticDataTypeId = IntegerDataType,
                            StatisticTypeId = statisticTypes.Single(t => t.Name == "Goals - For - Home").Id,
                            Value = goalStats.For.Total.Home.ToString(),
                        },
                        new()
                        {
                            StatisticDataTypeId = IntegerDataType,
                            StatisticTypeId = statisticTypes.Single(t => t.Name == "Goals - For - Away").Id,
                            Value = goalStats.For.Total.Away.ToString(),
                        },
                        new()
                        {
                            StatisticDataTypeId = IntegerDataType,
                            StatisticTypeId = statisticTypes.Single(t => t.Name == "Goals - For - Total").Id,
                            Value = goalStats.For.Total.Total.ToString(),
                        },
                        new()
                        {
                            StatisticDataTypeId = IntegerDataType,
                            StatisticTypeId = statisticTypes.Single(t => t.Name == "Goals - Against - Home").Id,
                            Value = goalStats.Against.Total.Home.ToString(),
                        },
                        new()
                        {
                            StatisticDataTypeId = IntegerDataType,
                            StatisticTypeId = statisticTypes.Single(t => t.Name == "Goals - Against - Away").Id,
                            Value = goalStats.Against.Total.Away.ToString(),
                        },
                        new()
                        {
                            StatisticDataTypeId = IntegerDataType,
                            StatisticTypeId = statisticTypes.Single(t => t.Name == "Goals - Against - Total").Id,
                            Value = goalStats.Against.Total.Total.ToString(),
                        },
                    };

                    foreach (var stat in stats)
                    {
                        await repository.Teams.CreateStatisticAsync(team.Id, league.Id, season.Year, date, stat, stoppingToken);
                    }
                }
            }
        }
    }
}
