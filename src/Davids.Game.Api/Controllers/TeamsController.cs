using Davids.Game.Api.DiscordAuth;
using Davids.Game.Models;
using Davids.Game.Models.Leagues;
using Davids.Game.Models.Statistics;
using Davids.Game.Models.Teams;
using Davids.Game.Models.Venues;
using Davids.Game.Teams;
using Microsoft.AspNetCore.Mvc;

namespace Davids.Game.Api.Controllers;

[Route("api/teams")]
[ApiController, DiscordAuthorize]
public class TeamsController(ITeamsRepository repository) : ControllerBase
{
    [HttpGet, Route("{teamId}")]
    [ProducesResponseType<TeamResponse>(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetTeamAsync(long teamId, CancellationToken cancellationToken)
    {
        var team = await repository.GetTeamAsync(teamId, cancellationToken);

        if (team == null) return NotFound();

        return Ok(team);
    }

    [HttpGet, Route("{teamId}/statistics/{season}")]
    public async Task<CollectionResponse<StatisticResponse>> GetStatisticsAsync(long teamId, string season, CancellationToken cancellationToken)
    {
        return (await repository.GetStatisticsAsync(teamId, season, cancellationToken)).ToCollectionResponse();
    }

    [HttpGet, Route("{teamId}/leagues")]
    public async Task<CollectionResponse<LeagueSeasonResponse>> GetLeaguesAsync(long teamId, CancellationToken cancellationToken)
    {
        return (await repository.GetLeaguesAsync(teamId, cancellationToken)).ToCollectionResponse();
    }

    [HttpGet, Route("{teamId}/venues")]
    public async Task<CollectionResponse<VenueResponse>> GetVenuesAsync(long teamId, CancellationToken cancellationToken)
    {
        return (await repository.GetVenuesAsync(teamId, cancellationToken)).ToCollectionResponse();
    }
}
