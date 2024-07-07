using Davids.Game.Api.DiscordAuth;
using Davids.Game.Models;
using Davids.Game.Models.Lists;
using Davids.Game.Models.Pools;
using Davids.Game.Models.UserPools;
using Davids.Game.Models.Users;
using Davids.Game.Pools;
using Davids.Game.Users;
using Microsoft.AspNetCore.Mvc;

namespace Davids.Game.Api.Controllers;

[Route("api/pools")]
[ApiController]
[DiscordAuthorize]
public class PoolsController(IPoolsRepository repository, IUserService userService) : ControllerBase
{
    [HttpGet, Route("{poolId}")]
    [ProducesResponseType<PoolResponse>(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetPoolAsync(long poolId, CancellationToken cancellationToken)
    {
        var user = await userService.GetUserAsync(this.User, cancellationToken);
        var userPool = await userService.GetUserPoolAsync(user.Id, poolId, cancellationToken);

        if (userPool == null) return Forbid();

        var pool = await repository.GetPoolAsync(poolId, cancellationToken);

        if (pool == null) return NotFound();

        return Ok(pool);
    }

    [HttpPost, Route("")]
    public async Task<ValueResponse<long>> CreatePoolAsync(PoolWriteRequest request, CancellationToken cancellationToken)
    {
        var id = await repository.CreatePoolAsync(request, cancellationToken);

        var user = await userService.GetUserAsync(this.User, cancellationToken);

        await repository.AddUserAsync(id, new() { Attributes = UserPoolAttribute.FirstUser, UserId = user.Id }, cancellationToken);

        return new(id);
    }

    [HttpPut, Route("{poolId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdatePoolAsync(long poolId, PoolWriteRequest request, CancellationToken cancellationToken)
    {
        var user = await userService.GetUserAsync(this.User, cancellationToken);
        var userPool = await userService.GetUserPoolAsync(user.Id, poolId, cancellationToken);

        if (userPool == null) return Forbid();

        if (!userPool.IsAdmin) return Forbid();

        if (!await repository.UpdatePoolAsync(poolId, request, cancellationToken)) return NotFound();

        return NoContent();
    }

    [HttpDelete, Route("{poolId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeletePoolAsync(long poolId, CancellationToken cancellationToken)
    {
        var user = await userService.GetUserAsync(this.User, cancellationToken);
        var userPool = await userService.GetUserPoolAsync(user.Id, poolId, cancellationToken);

        if (userPool == null) return Forbid();

        if (!userPool.IsOwner) return Forbid();

        if (!await repository.DeletePoolAsync(poolId, cancellationToken)) return NotFound();

        return NoContent();
    }

    [HttpGet, Route("{poolId}/users")]
    [ProducesResponseType<CollectionResponse<PoolUserResponse>>(200)]
    public async Task<IActionResult> GetUsersAsync(long poolId, CancellationToken cancellationToken)
    {
        var user = await userService.GetUserAsync(this.User, cancellationToken);
        var userPool = await userService.GetUserPoolAsync(user.Id, poolId, cancellationToken);

        if (userPool == null) return Forbid();

        return Ok((await repository.GetUsersAsync(poolId, cancellationToken)).ToCollectionResponse());
    }

    [HttpGet, Route("{poolId}/lists")]
    [ProducesResponseType<CollectionResponse<ListResponse>>(200)]
    public async Task<IActionResult> GetListsAsync(long poolId, CancellationToken cancellationToken)
    {
        var user = await userService.GetUserAsync(this.User, cancellationToken);
        var userPool = await userService.GetUserPoolAsync(user.Id, poolId, cancellationToken);

        if (userPool == null) return Forbid();

        return Ok((await repository.GetListsAsync(poolId, cancellationToken)).ToCollectionResponse());
    }

    [HttpPost, Route("{poolId}/join/{code}")]
    [ProducesResponseType<ValueResponse<long>>(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> JoinAsync(long poolId, string code, CancellationToken cancellationToken)
    {
        var user = await userService.GetUserAsync(this.User, cancellationToken);
        var pool = await repository.GetPoolAsync(poolId, cancellationToken);

        if (pool == null) return NotFound();

        if (!string.Equals(pool.JoinCode, code, StringComparison.OrdinalIgnoreCase)) return Forbid();

        return Ok(new ValueResponse<long>(await repository.AddUserAsync(poolId, new() { Attributes = UserPoolAttribute.None, UserId = user.Id }, cancellationToken)));
    }

    [HttpDelete, Route("{poolId}/users/{userId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> RemoveUserAsync(long poolId, long userId, CancellationToken cancellationToken)
    {
        var user = await userService.GetUserAsync(this.User, cancellationToken);
        var userPool = await userService.GetUserPoolAsync(user.Id, poolId, cancellationToken);

        if (userPool == null) return Forbid();

        if (!userPool.IsAdmin) return Forbid();

        var targetUserPool = await userService.GetUserPoolAsync(userId, poolId, cancellationToken);

        if (targetUserPool == null) return NotFound();

        if (targetUserPool.IsOwner) return Forbid();

        if (targetUserPool.IsAdmin && !userPool.IsOwner) return Forbid();

        if (!await repository.RemoveUserAsync(poolId, userId, cancellationToken)) return NotFound();

        return NoContent();
    }
}
