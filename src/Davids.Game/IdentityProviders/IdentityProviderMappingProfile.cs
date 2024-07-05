using AutoMapper;
using Davids.Game.Data;
using Davids.Game.Models.IdentityProviders;

namespace Davids.Game.IdentityProviders;

public class IdentityProviderMappingProfile : Profile
{
    public IdentityProviderMappingProfile() : base()
    {
        CreateMap<IdentityProvider, IdentityProviderResponse>();
    }
}
