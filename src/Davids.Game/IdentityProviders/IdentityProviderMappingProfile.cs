using AutoMapper;
using Davids.Game.Data;
using Davids.Game.Models;

namespace Davids.Game.IdentityProviders;

public class IdentityProviderMappingProfile : Profile
{
    public IdentityProviderMappingProfile() : base()
    {
        CreateMap<IdentityProvider, EnumerationResponse>();
    }
}
