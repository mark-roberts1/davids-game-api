using Davids.Game.Api.DiscordAuth;
using Davids.Game.Lists;
using Davids.Game.Models;
using Davids.Game.Models.Lists;
using Davids.Game.Users;
using Microsoft.AspNetCore.Mvc;

namespace Davids.Game.Api.Controllers;

[Route("api/lists")]
[ApiController]
[DiscordAuthorize]
public class ListsController(IListsRepository repository, IUserService userService) : ControllerBase
{
    [HttpGet, Route("{listId}")]
    [ProducesResponseType<ListResponse>(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetListAsync(long listId, CancellationToken cancellationToken)
    {
        var list = await repository.GetListAsync(listId, cancellationToken);

        if (list == null) return NotFound();

        return Ok(list);
    }

    [HttpPost, Route("")]
    [ProducesResponseType<ValueResponse<long>>(200)]
    public async Task<IActionResult> CreateListAsync(ListWriteRequest request, CancellationToken cancellationToken)
    {
        var user = await userService.GetUserAsync(this.User, cancellationToken);
        var pool = await userService.GetUserPoolAsync(user.Id, request.PoolId, cancellationToken);

        if (pool == null) return Forbid();

        var id = await repository.CreateListAsync(pool.UserPoolId, request, cancellationToken);

        return Ok(new ValueResponse<long>(id));
    }

    [HttpPut, Route("{listId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateListAsync(long listId, ListWriteRequest request, CancellationToken cancellationToken)
    {
        var user = await userService.GetUserAsync(this.User, cancellationToken);
        var pool = await userService.GetUserPoolAsync(user.Id, request.PoolId, cancellationToken);

        if (pool == null) return Forbid();

        var list = await repository.GetListAsync(listId, cancellationToken);

        if (list == null) return NotFound();

        if (list.UserPoolId != pool.UserPoolId) return Forbid();

        if (!await repository.UpdateListAsync(listId, pool.UserPoolId, request, cancellationToken)) return NotFound();

        return NoContent();
    }

    [HttpDelete, Route("{listId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteListAsync(long listId, CancellationToken cancellationToken)
    {
        var user = await userService.GetUserAsync(this.User, cancellationToken);
        
        var list = await repository.GetListAsync(listId, cancellationToken);

        if (list == null) return NotFound();

        var pool = await userService.GetUserPoolAsync(list.UserPoolId, cancellationToken);

        if (pool == null) return NotFound();

        if (pool.UserId != user.Id) return Forbid();

        if (!await repository.DeleteListAsync(listId, cancellationToken)) return NotFound();

        return NoContent();
    }

}
