using Davids.Game.Api.DiscordAuth;
using Davids.Game.Leagues;
using Davids.Game.Models;
using Davids.Game.Models.Leagues;
using Davids.Game.Models.Teams;
using Microsoft.AspNetCore.Mvc;

namespace Davids.Game.Api.Controllers;

[Route("api/leagues")]
[ApiController]
public class LeaguesController(ILeaguesRepository repository) : ControllerBase
{
    [HttpPost, Route("search"), DiscordAuthorize]
    public async Task<PageResponse<LeagueResponse>> SearchLeaguesAsync(LeagueSearchRequest request, CancellationToken cancellationToken)
        => await repository.SearchLeaguesAsync(request, cancellationToken);

    [HttpGet, Route("{leagueId}"), DiscordAuthorize]
    [ProducesResponseType<LeagueResponse>(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetLeagueAsync(int leagueId, CancellationToken cancellationToken)
    {
        var league = await repository.GetLeagueAsync(leagueId, cancellationToken);

        if (league == null) return NotFound();

        return Ok(league);
    }

    [HttpGet, Route("{leagueId}/teams"), DiscordAuthorize]
    public async Task<PageResponse<TeamResponse>> GetTeamsAsync(int leagueId, string? season, int? pageNumber, int? pageSize, CancellationToken cancellationToken)
        => await repository.GetTeamsAsync(leagueId, season, pageNumber, pageSize, cancellationToken);
}
