using Davids.Game.Api.DiscordAuth;
using Davids.Game.Models;
using Davids.Game.Models.UserPools;
using Davids.Game.Models.Users;
using Davids.Game.Users;
using Microsoft.AspNetCore.Mvc;

namespace Davids.Game.Api.Controllers;

[Route("api/users")]
[ApiController, DiscordAuthorize]
public class UsersController(IUserService service, IUsersRepository repository) : ControllerBase
{
    [HttpGet, Route("me")]
    public async Task<UserResponse> GetSelfAsync(CancellationToken cancellationToken)
    {
        return await service.GetUserAsync(this.User, cancellationToken);
    }

    [HttpGet, Route("me/pools")]
    public async Task<CollectionResponse<UserPoolResponse>> GetPoolsAsync(CancellationToken cancellationToken)
    {
        var user = await service.GetUserAsync(this.User, cancellationToken);

        return (await repository.GetPoolsAsync(user.Id, cancellationToken)).ToCollectionResponse();
    }
}
