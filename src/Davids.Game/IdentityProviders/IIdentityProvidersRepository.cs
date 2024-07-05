using Davids.Game.Models.IdentityProviders;

namespace Davids.Game.IdentityProviders;
public interface IIdentityProvidersRepository
{
    Task<IEnumerable<IdentityProviderResponse>> GetIdentityProvidersAsync(CancellationToken cancellationToken);
}
