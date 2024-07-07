using Davids.Game.Models.SportsApi;
using Davids.Game.SportsApi;

namespace Davids.Game.Api.HostedServices;

public class DataPrimerService(SportsApiHttpClient sportsApi, IRepository repository, IConfiguration configuration) : BackgroundService
{
    private bool Enabled => configuration.GetValue<bool>("DataLoader:Enabled");

    private DateTime lastRan = DateTime.MinValue;

    private IEnumerable<string> GetSeasonIds()
    {
        var current = DateTime.Now.Year;

        for (var year = current - 2; year < current + 2; year++)
            yield return year.ToString();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!Enabled) return;

        while (!stoppingToken.IsCancellationRequested)
        {
            if (DateTime.UtcNow.AddHours(-24) > lastRan)
            {
                foreach (var season in GetSeasonIds())
                {
                    try
                    {
                        var leagues = await sportsApi.GetLeaguesAsync(season, stoppingToken);

                        foreach (var league in leagues.Response)
                        {
                            try
                            {
                                var leagueId = await SaveLeagueAsync(league, stoppingToken);

                                var teams = await sportsApi.GetTeamsAsync(league.League.Id, season, stoppingToken);

                                foreach (var team in teams.Response)
                                {
                                    try
                                    {
                                        await SaveTeamAsync(season, leagueId, team, stoppingToken);
                                    }
                                    catch
                                    {
                                        await Task.Delay(60_000, stoppingToken);
                                    }
                                }
                            }
                            catch
                            {
                                await Task.Delay(60_000, stoppingToken);
                            }

                        }
                    }
                    catch
                    {
                        await Task.Delay(60_000, stoppingToken);
                    }
                }

                lastRan = DateTime.UtcNow;
            }

            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task<int> SaveLeagueAsync(LeagueResponseItem leagueItem, CancellationToken cancellationToken)
    {
        var leagueRequest = leagueItem.League;
        var countryRequest = leagueItem.Country;

        var league = (await repository.Leagues.SearchLeaguesAsync(
            new()
            {
                PageNumber = 1,
                PageSize = 1,
                SourceId = leagueRequest.Id,
            },
            cancellationToken))
            .Page.SingleOrDefault();

        var types = await repository.Leagues.GetLeagueTypesAsync(cancellationToken);
        var type = types.SingleOrDefault(t => t.Name == leagueRequest.Type);

        var country = await repository.Countries.GetCountryAsync(name: countryRequest.Name, code: null, cancellationToken);

        if (countryRequest.Name != "World")
        {
            country ??= new()
            {
                Id = await repository.Countries.CreateCountryAsync(
            new()
            {
                Code = countryRequest.Code,
                Name = countryRequest.Name,
                FlagLink = countryRequest.Flag,
            },
            cancellationToken),

                Name = countryRequest.Name,
                Code = countryRequest.Code,
                FlagLink = countryRequest.Flag
            };
        }

        type ??= new()
        {
            Id = await repository.Leagues.AddLeagueTypeAsync(leagueRequest.Type, cancellationToken),
            Name = leagueRequest.Type
        };

        league ??= new()
        {
            Id = await repository.Leagues.CreateLeagueAsync(
                new()
                {
                    CountryId = country?.Id,
                    SourceId = leagueRequest.Id,
                    LeagueTypeId = type.Id,
                    LogoLink = leagueRequest.Logo,
                    Name = leagueRequest.Name,
                },
                cancellationToken),
            CountryId = country?.Id,
            SourceId = leagueRequest.Id,
            LeagueTypeId = type.Id,
            LogoLink = leagueRequest.Logo,
            Name = leagueRequest.Name,
        };

        if (league.CountryId != country?.Id || league.Name != leagueRequest.Name || league.LeagueTypeId != type.Id || league.LogoLink != leagueRequest.Logo)
        {
            await repository.Leagues.UpdateLeagueAsync(
                league.Id,
                new()
                {
                    CountryId = country?.Id,
                    SourceId = leagueRequest.Id,
                    LeagueTypeId = type.Id,
                    LogoLink = leagueRequest.Logo,
                    Name = leagueRequest.Name,
                },
                cancellationToken);
        }

        return league.Id;
    }

    private async Task SaveTeamAsync(string season, int leagueId, TeamResponseItem teamItem, CancellationToken cancellationToken)
    {
        var teamRequest = teamItem.Team;
        var venueRequest = teamItem.Venue;

        var country = teamRequest.Country == null ? null : await repository.Countries.GetCountryAsync(name: teamRequest.Country, code: null, cancellationToken);

        var team = await repository.Teams.GetTeamBySourceIdAsync(teamRequest.Id, cancellationToken);

        team ??= new()
        {
            Id = await repository.Teams.CreateTeamAsync(
                new()
                {
                    Code = teamRequest.Code,
                    CountryId = country?.Id,
                    SourceId = teamRequest.Id,
                    LogoLink = teamRequest.Logo,
                    Name = teamRequest.Name,
                },
                cancellationToken),
            Code = teamRequest.Code,
            CountryId = country?.Id,
            SourceId = teamRequest.Id,
            LogoLink = teamRequest.Logo,
            Name = teamRequest.Name,
        };

        if (
            team.Code != teamRequest.Code
            || team.CountryId != country?.Id
            || team.SourceId != teamRequest.Id
            || team.LogoLink != teamRequest.Logo
            || team.Name != teamRequest.Name)
        {
            await repository.Teams.UpdateTeamAsync(
                team.Id,
                new()
                {
                    Code = teamRequest.Code,
                    CountryId = country?.Id,
                    SourceId = teamRequest.Id,
                    LogoLink = teamRequest.Logo,
                    Name = teamRequest.Name,
                },
                cancellationToken);
        }

        if (venueRequest.Id.HasValue)
        {
            var surfaceTypes = await repository.Venues.GetSurfaceTypesAsync(cancellationToken);
            var surfaceType = surfaceTypes.SingleOrDefault(s => s.Name == venueRequest.Surface);

            if (venueRequest.Surface != null)
            {
                surfaceType ??= new()
                {
                    Id = await repository.Venues.CreateSurfaceTypeAsync(venueRequest.Surface, cancellationToken),
                    Name = venueRequest.Surface,
                };
            }
            
            var teamVenues = await repository.Teams.GetVenuesAsync(team.Id, cancellationToken);
            var venue = venueRequest.Id.HasValue ? await repository.Venues.GetVenueBySourceIdAsync(venueRequest.Id.Value, cancellationToken) : null;

            venue ??= new()
            {
                Id = await repository.Venues.CreateVenueAsync(
                new()
                {
                    SourceId = venueRequest.Id,
                    SurfaceTypeId = surfaceType?.Id,
                    Capacity = venueRequest.Capacity,
                    ImageLink = venueRequest.Image,
                    Name = venueRequest.Name ?? $"{team.Name ?? "Unknown"} Venue",
                },
                cancellationToken),
                SourceId = venueRequest.Id,
                SurfaceTypeId = surfaceType?.Id,
                Capacity = venueRequest.Capacity,
                ImageLink = venueRequest.Image,
                Name = venueRequest.Name ?? $"{team.Name ?? "Unknown"} Venue",
            };

            if (
                venue.SourceId != venueRequest.Id
                || venue.SurfaceTypeId != surfaceType?.Id
                || venue.Capacity != venueRequest.Capacity
                || venue.ImageLink != venueRequest.Image
                || venue.Name != (venueRequest.Name ?? $"{team.Name ?? "Unknown"} Venue"))
            {
                await repository.Venues.UpdateVenueAsync(
                    venue.Id,
                    new()
                    {
                        SourceId = venueRequest.Id,
                        SurfaceTypeId = surfaceType?.Id,
                        Capacity = venueRequest.Capacity,
                        ImageLink = venueRequest.Image,
                        Name = venueRequest.Name ?? $"{team.Name ?? "Unknown"} Venue",
                    },
                    cancellationToken);
            }

            if (!teamVenues.Any(v => v.Id == venue.Id))
            {
                await repository.Teams.AddVenueAsync(team.Id, venue.Id, cancellationToken);
            }
        }

        var leagues = await repository.Teams.GetLeaguesAsync(team.Id, cancellationToken);

        if (!leagues.Any(l => l.Season == season && l.Id == leagueId))
        {
            await repository.Teams.AddLeagueAsync(team.Id, leagueId, season, cancellationToken);
        }
    }
}
