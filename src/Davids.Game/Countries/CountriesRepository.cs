using AutoMapper;
using Davids.Game.Data;
using Davids.Game.DependencyInjection;
using Davids.Game.Models.Countries;
using Microsoft.EntityFrameworkCore;

namespace Davids.Game.Countries;

[Service<ICountriesRepository>]
internal class CountriesRepository(
    IDbContextFactory<DavidsGameContext> contextFactory,
    IMapper mapper) : ICountriesRepository
{
    public async Task<short> CreateCountryAsync(CountryWriteRequest request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var entry = mapper.Map<Country>(request);

        await context.Countries.AddAsync(entry, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return entry.Id;
    }

    public async Task<IEnumerable<CountryResponse>> GetCountriesAsync(CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        return await context.Countries.Select(c => mapper.Map<CountryResponse>(c)).ToListAsync(cancellationToken);
    }

    public async Task<CountryResponse?> GetCountryAsync(short countryId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var country = await context.Countries.SingleOrDefaultAsync(c => c.Id == countryId, cancellationToken);

        if (country == null) return null;

        return mapper.Map<CountryResponse>(country);
    }

    public async Task<CountryResponse?> GetCountryAsync(string? name, string? code, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var query = context.Countries.AsQueryable();

        if (name != null)
        {
            query = query.Where(c => c.Name == name);
        }

        if (code != null)
        {
            query = query.Where(c => c.Code == code);
        }

        var country = await query.SingleOrDefaultAsync(cancellationToken);

        if (country == null) return null;

        return mapper.Map<CountryResponse>(country);
    }
}
