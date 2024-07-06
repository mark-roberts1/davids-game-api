using Davids.Game.Api.DiscordAuth;
using Davids.Game.Models;
using Davids.Game.Statistics;
using Microsoft.AspNetCore.Mvc;

namespace Davids.Game.Api.Controllers;

[Route("api/statistics")]
[ApiController]
public class StatisticsController(IStatisticsRepository repository) : ControllerBase
{
    [HttpGet, Route("types"), DiscordAuthorize]
    public async Task<CollectionResponse<EnumerationResponse>> GetTypesAsync(CancellationToken cancellationToken)
    {
        return (await repository.GetStatisticTypesAsync(cancellationToken)).ToCollectionResponse();
    }

    [HttpGet, Route("data-types"), DiscordAuthorize]
    public async Task<CollectionResponse<EnumerationResponse>> GetDataTypesAsync(CancellationToken cancellationToken)
    {
        return (await repository.GetStatisticDataTypesAsync(cancellationToken)).ToCollectionResponse();
    }
}
