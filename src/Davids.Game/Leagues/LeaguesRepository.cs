using AutoMapper;
using Davids.Game.Data;
using Davids.Game.DependencyInjection;
using Davids.Game.Extensions;
using Davids.Game.Models;
using Davids.Game.Models.Leagues;
using Davids.Game.Models.Teams;
using Microsoft.EntityFrameworkCore;

namespace Davids.Game.Leagues;

[Service<ILeaguesRepository>]
internal class LeaguesRepository(IDbContextFactory<DavidsGameContext> contextFactory, IMapper mapper) : ILeaguesRepository
{
    public async Task<int> CreateLeagueAsync(LeagueWriteRequest request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<League>(request);

        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        await context.Leagues.AddAsync(entity, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task<bool> DeleteLeagueAsync(int leagueId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var league = context.Leagues.FindLocalOrAttach(l => l.Id == leagueId, new() { Id = leagueId });

        try
        {
            context.Leagues.Remove(league);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public async Task<LeagueResponse?> GetLeagueAsync(int leagueId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var league = await context.Leagues.SingleOrDefaultAsync(l => l.Id == leagueId, cancellationToken);

        if (league == null) return null;

        return mapper.Map<LeagueResponse?>(league);
    }

    public async Task<PageResponse<TeamResponse>> GetTeamsAsync(int leagueId, string? season, int? pageNumber, int? pageSize, CancellationToken cancellationToken)
    {
        pageNumber ??= 1;
        pageSize ??= 100;

        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var query = context
            .TeamSeasonLeagues
                .Include(tsl => tsl.Team)
                .ThenInclude(t => t.Country)
            .Where(tsl => tsl.LeagueId == leagueId)
            .AsQueryable();

        if (season != null)
        {
            query = query.Where(tsl => tsl.Season == season);
        }

        var teamQuery = query
            .Select(tsl => tsl.Team)
            .Distinct();

        var total = await teamQuery.CountAsync(cancellationToken);

        var results = await teamQuery
            .OrderBy(t => t.Name)
            .Skip((pageNumber.Value - 1) * pageSize.Value)
            .Take(pageSize.Value)
            .Select(t => mapper.Map<TeamResponse>(t))
            .ToArrayAsync(cancellationToken);

        return results.ToPageResponse(total);
    }

    public async Task<PageResponse<LeagueResponse>> SearchLeaguesAsync(LeagueSearchRequest request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var query = context.Leagues.Include(l => l.Country).AsQueryable();

        if (request.Name != null)
        {
            query = query.Where(l => l.Name == request.Name);
        }

        if (request.LeagueType != null)
        {
            query = query.Where(l => l.Type == (short)request.LeagueType);
        }

        if (request.CountryId != null)
        {
            query = query.Where(l => l.CountryId == request.CountryId);
        }

        if (request.SourceId != null)
        {
            query = query.Where(l => l.SourceId == request.SourceId);
        }

        var count = await query.CountAsync(cancellationToken);

        var results = await query
            .OrderBy(l => l.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(l => mapper.Map<LeagueResponse>(l))
            .ToListAsync(cancellationToken);

        return results.ToPageResponse(count);
    }

    public async Task<bool> UpdateLeagueAsync(int leagueId, LeagueWriteRequest request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var league = await context.Leagues.SingleOrDefaultAsync(l => l.Id == leagueId, cancellationToken);

        if (league == null) return false;

        context.Entry(league).CurrentValues.SetValues(request);

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
