using Davids.Game.Data;
using Davids.Game.DependencyInjection;
using Davids.Game.Models.Statistics;
using Microsoft.EntityFrameworkCore;

namespace Davids.Game.Statistics;

[Service<IStatisticsRepository>]
internal class StatisticsRepository(IDbContextFactory<DavidsGameContext> contextFactory) : IStatisticsRepository
{
    public async Task<IEnumerable<EnumerationResponse>> GetStatisticDataTypesAsync(CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        return await context
            .StatisticDataTypes
            .Select(s => new EnumerationResponse { Id = s.Id, Name = s.Name })
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<EnumerationResponse>> GetStatisticTypesAsync(CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        return await context
            .StatisticTypes
            .Select(s => new EnumerationResponse { Id = s.Id, Name = s.Name })
            .ToListAsync(cancellationToken);
    }
}
