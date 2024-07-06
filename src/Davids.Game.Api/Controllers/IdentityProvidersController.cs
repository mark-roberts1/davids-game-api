using Davids.Game.IdentityProviders;
using Davids.Game.Models;
using Microsoft.AspNetCore.Mvc;

namespace Davids.Game.Api.Controllers;

[Route("api/identity-providers")]
[ApiController]
public class IdentityProvidersController(IIdentityProvidersRepository repository) : ControllerBase
{
    [HttpGet, Route("")]
    public async Task<CollectionResponse<EnumerationResponse>> GetIdentityProvidersAsync(CancellationToken cancellationToken)
    {
        return (await repository.GetIdentityProvidersAsync(cancellationToken)).ToCollectionResponse();
    }
}
