using AutoMapper;
using Davids.Game.Data;
using Davids.Game.DependencyInjection;
using Davids.Game.Extensions;
using Davids.Game.Models.Leagues;
using Davids.Game.Models.Statistics;
using Davids.Game.Models.Teams;
using Davids.Game.Models.Venues;
using Microsoft.EntityFrameworkCore;

namespace Davids.Game.Teams;

[Service<ITeamsRepository>]
internal class TeamsRepository(IDbContextFactory<DavidsGameContext> contextFactory, IMapper mapper) : ITeamsRepository
{
    public async Task<bool> AddLeagueAsync(long teamId, int leagueId, string season, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        context.TeamSeasonLeagues.Add(new()
        {
            Season = season,
            LeagueId = leagueId,
            TeamId = teamId
        });

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<long> CreateTeamAsync(TeamWriteRequest request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var team = mapper.Map<Team>(request);

        context.Teams.Add(team);
        await context.SaveChangesAsync(cancellationToken);

        return team.Id;
    }

    public async Task<bool> DeleteTeamAsync(long teamId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var team = context.Teams.FindLocalOrAttach(t => t.Id == teamId, new() { Id = teamId });

        try
        {
            context.Teams.Remove(team);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public async Task<IEnumerable<LeagueSeasonResponse>> GetLeaguesAsync(long teamId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        return await context
            .TeamSeasonLeagues
                .Include(t => t.League)
            .Where(t => t.TeamId == teamId)
            .Select(t => mapper.Map<LeagueSeasonResponse>(t))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<StatisticResponse>> GetStatisticsAsync(long teamId, string season, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        return await context
            .TeamSeasonStatistics
                .Include(tss => tss.Statistic)
            .Where(tss => tss.TeamId == teamId && tss.Season == season)
            .Distinct()
            .Select(tss => mapper.Map<StatisticResponse>(tss.Statistic))
            .ToListAsync(cancellationToken);
    }

    public async Task<TeamResponse?> GetTeamAsync(long teamId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var team = await context.Teams.SingleOrDefaultAsync(t => t.Id == teamId, cancellationToken);

        if (team == null) return null;

        return mapper.Map<TeamResponse>(team);
    }

    public async Task<IEnumerable<VenueResponse>> GetVenuesAsync(long teamId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        return await context
            .Venues
            .Where(v => v.Teams.Any(t => t.Id == teamId))
            .Select(v => mapper.Map<VenueResponse>(v))
            .ToListAsync(cancellationToken);
    }

    public async Task SaveStatisticsAsync(long teamId, string season, IEnumerable<StatisticWriteRequest> requests, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var tss = await context.TeamSeasonStatistics.Where(tss => tss.TeamId == teamId &&  tss.Season == season).ToListAsync(cancellationToken);

        if (tss != null)
        {
            context.TeamSeasonStatistics.RemoveRange(tss);
        }

        var statistics = requests.Select(r => mapper.Map<Statistic>(r)).ToList();

        tss = statistics.Select(s => new TeamSeasonStatistic { Statistic = s, TeamId = teamId, Season = season }).ToList();

        context.TeamSeasonStatistics.AddRange(tss);

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> UpdateTeamAsync(long teamId, TeamWriteRequest request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var team = await context.Teams.SingleOrDefaultAsync(t => t.Id == teamId, cancellationToken);

        if (team == null) return false;

        context.Entry(team).CurrentValues.SetValues(request);

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
