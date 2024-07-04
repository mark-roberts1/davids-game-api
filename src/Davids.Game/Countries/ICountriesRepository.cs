using Davids.Game.Models.Countries;

namespace Davids.Game.Countries;

public interface ICountriesRepository
{
    Task<IEnumerable<CountryResponse>> GetCountriesAsync(CancellationToken cancellationToken);
    Task<CountryResponse?> GetCountryAsync(short countryId, CancellationToken cancellationToken);
    Task<CountryResponse?> GetCountryAsync(string? name, string? code, CancellationToken cancellationToken);
    Task<short> CreateCountryAsync(CountryWriteRequest request, CancellationToken cancellationToken);
}
