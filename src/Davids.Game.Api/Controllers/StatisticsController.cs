using Davids.Game.Api.DiscordAuth;
using Davids.Game.Models;
using Davids.Game.Models.Leagues;
using Davids.Game.Statistics;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;

namespace Davids.Game.Api.Controllers;

[Route("api/statistics")]
[ApiController]
public class StatisticsController(IStatisticsRepository repository, ChannelWriter<LeagueSeasonStatsRequest> queue) : ControllerBase
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

    [HttpPost, Route("sync"), DiscordAuthorize]
    public async Task RunSyncCommandAsync(LeagueSeasonStatsRequest request, CancellationToken cancellationToken)
    {
        await queue.WriteAsync(request, cancellationToken);
    }
}
