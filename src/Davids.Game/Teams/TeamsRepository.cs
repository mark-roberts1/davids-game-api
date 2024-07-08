﻿using AutoMapper;
using Davids.Game.Data;
using Davids.Game.DependencyInjection;
using Davids.Game.Extensions;
using Davids.Game.Models.Leagues;
using Davids.Game.Models.Teams;
using Davids.Game.Models.Venues;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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

    public async Task AddVenueAsync(long teamId, long venueId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        using var connection = context.Database.GetDbConnection();
        using var command = connection.CreateCommand();

        command.CommandText = "INSERT INTO game.team_venue(team_id, venue_id) VALUES (@team_id, @venue_id);";
        command.Parameters.Add(new NpgsqlParameter("@team_id", teamId));
        command.Parameters.Add(new NpgsqlParameter("@venue_id", venueId));

        await connection.OpenAsync(cancellationToken);
        await command.ExecuteNonQueryAsync(cancellationToken);
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

    public async Task<TeamResponse?> GetTeamAsync(long teamId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var team = await context.Teams.SingleOrDefaultAsync(t => t.Id == teamId, cancellationToken);

        if (team == null) return null;

        return mapper.Map<TeamResponse>(team);
    }

    public async Task<TeamResponse?> GetTeamBySourceIdAsync(long sourceId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var team = await context.Teams.SingleOrDefaultAsync(t => t.SourceId == sourceId, cancellationToken);

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
