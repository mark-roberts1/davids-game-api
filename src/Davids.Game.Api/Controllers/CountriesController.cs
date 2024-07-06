using Davids.Game.Api.DiscordAuth;
using Davids.Game.Countries;
using Davids.Game.Models;
using Davids.Game.Models.Countries;
using Microsoft.AspNetCore.Mvc;

namespace Davids.Game.Api.Controllers;

[Route("api/countries")]
[ApiController]
public class CountriesController(ICountriesRepository repository) : ControllerBase
{
    [HttpGet, Route(""), DiscordAuthorize]
    public async Task<CollectionResponse<CountryResponse>> GetCountriesAsync(CancellationToken cancellationToken)
    {
        return (await repository.GetCountriesAsync(cancellationToken)).ToCollectionResponse();
    }
}
