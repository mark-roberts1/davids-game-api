using Davids.Game.Models.SportsApi;
using System.Diagnostics;
using System.Text.Json;

namespace Davids.Game.SportsApi;

public class SportsApiHttpClient(HttpClient client, SportsApiConfig configuration)
{
    // api is rate limited at 300 requests per minute. 
    const int MinTime = 200;

    public async Task<LeagueResponse> GetLeaguesAsync(string season, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"leagues?season={season}");

            request.Headers.Add("x-rapidapi-host", "v3.football.api-sports.io");
            request.Headers.Add("x-rapidapi-key", configuration.Token);

            var response = await client.SendAsync(request, cancellationToken);

            var json = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<LeagueResponse>(json)!;
        }
        finally
        {
            sw.Stop();

            var timeLeft = MinTime - sw.ElapsedMilliseconds;

            if (timeLeft > 0)
            {
                await Task.Delay((int)timeLeft, cancellationToken);
            }
        }
    }

    public async Task<TeamResponse> GetTeamsAsync(long leagueId, string season, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"teams?league={leagueId}&season={season}");

            request.Headers.Add("x-rapidapi-host", "v3.football.api-sports.io");
            request.Headers.Add("x-rapidapi-key", configuration.Token);

            var response = await client.SendAsync(request, cancellationToken);

            var json = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<TeamResponse>(json)!;
        }
        finally
        {
            sw.Stop();

            var timeLeft = MinTime - sw.ElapsedMilliseconds;

            if (timeLeft > 0)
            {
                await Task.Delay((int)timeLeft, cancellationToken);
            }
        }
    }

    public async Task<StatisticsReponse> GetStatisticsAsync(short season, long leagueId, long teamId, DateTime date, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"teams/statistics?league={leagueId}&season={season}&team={teamId}&date={date:yyyy-MM-dd}");

            request.Headers.Add("x-rapidapi-host", "v3.football.api-sports.io");
            request.Headers.Add("x-rapidapi-key", configuration.Token);

            var response = await client.SendAsync(request, cancellationToken);

            var json = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<StatisticsReponse>(json)!;
        }
        finally
        {
            sw.Stop();

            var timeLeft = MinTime - sw.ElapsedMilliseconds;

            if (timeLeft > 0)
            {
                await Task.Delay((int)timeLeft, cancellationToken);
            }
        }
    }
}
