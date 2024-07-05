using Davids.Game.Models.Statistics;

namespace Davids.Game.Statistics;

public interface IStatisticsRepository
{
    Task<IEnumerable<EnumerationResponse>> GetStatisticTypesAsync(CancellationToken cancellationToken);
    Task<IEnumerable<EnumerationResponse>> GetStatisticDataTypesAsync(CancellationToken cancellationToken);
}
