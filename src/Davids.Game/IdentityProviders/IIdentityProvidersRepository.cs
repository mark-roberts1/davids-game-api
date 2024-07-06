using Davids.Game.Models;

namespace Davids.Game.IdentityProviders;
public interface IIdentityProvidersRepository
{
    Task<IEnumerable<EnumerationResponse>> GetIdentityProvidersAsync(CancellationToken cancellationToken);
}
