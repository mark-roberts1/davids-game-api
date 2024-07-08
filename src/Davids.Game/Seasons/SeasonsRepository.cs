using AutoMapper;
using Davids.Game.Data;
using Davids.Game.DependencyInjection;
using Davids.Game.Extensions;
using Davids.Game.Models.Leagues;
using Davids.Game.Models.Seasons;
using Microsoft.EntityFrameworkCore;

namespace Davids.Game.Seasons;

[Service<ISeasonsRepository>]
internal class SeasonsRepository(IDbContextFactory<DavidsGameContext> contextFactory, IMapper mapper) : ISeasonsRepository
{
    public async Task CreateSeasonAsync(int leagueId, short year, SeasonWriteRequest request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var entity = mapper.Map<Season>(request);

        entity.LeagueId = leagueId;
        entity.Year = year;

        context.Seasons.Add(entity);

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteSeasonAsync(int leagueId, short year, CancellationToken cancellationToken)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

            var entity = context.Seasons.FindLocalOrAttach(s => s.LeagueId == leagueId && s.Year == year, new() { LeagueId = leagueId, Year = year });

            context.Seasons.Remove(entity);

            await context.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public async Task<IEnumerable<LeagueResponse>> GetLeaguesAsync(short year, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        return await context
            .Seasons
                .Include(s => s.League)
                .ThenInclude(l => l.Country)
            .Where(s => s.Year == year)
            .Select(s => mapper.Map<LeagueResponse>(s.League))
            .ToListAsync(cancellationToken);
    }

    public async Task<SeasonResponse?> GetSeasonAsync(int leagueId, short year, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        return await context
            .Seasons
            .Where(s => s.LeagueId == leagueId && s.Year == year)
            .Select(s => mapper.Map<SeasonResponse>(s))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<SeasonResponse>> GetSeasonsAsync(int leagueId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        return await context
            .Seasons
            .Where(s => s.LeagueId == leagueId)
            .Select(s => mapper.Map<SeasonResponse>(s))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> UpdateSeasonAsync(int leagueId, short year, SeasonWriteRequest request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var entity = await context
            .Seasons
            .Where(s => s.LeagueId == leagueId && s.Year == year)
            .Select(s => mapper.Map<SeasonResponse>(s))
            .SingleOrDefaultAsync(cancellationToken);

        if (entity == null) return false;

        context.Entry(entity).CurrentValues.SetValues(request);

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
