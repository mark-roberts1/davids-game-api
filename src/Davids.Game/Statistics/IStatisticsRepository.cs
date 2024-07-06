using Davids.Game.Models;

namespace Davids.Game.Statistics;

public interface IStatisticsRepository
{
    Task<IEnumerable<EnumerationResponse>> GetStatisticTypesAsync(CancellationToken cancellationToken);
    Task<IEnumerable<EnumerationResponse>> GetStatisticDataTypesAsync(CancellationToken cancellationToken);
}
