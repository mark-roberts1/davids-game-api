using AutoMapper;
using Davids.Game.Data;
using Davids.Game.Models.Pools;
using Davids.Game.Models.UserPools;
using Davids.Game.Models.Users;

namespace Davids.Game.Pools;

public class PoolMappingProfile : Profile
{
    public PoolMappingProfile() : base()
    {
        CreateMap<Pool, PoolResponse>();

        CreateMap<UserPool, PoolUserResponse>()
            .ForMember(dest => dest.Attributes, opt => opt.MapFrom(src => (UserPoolAttribute)src.Attributes))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.User.Id))
            .ForMember(dest => dest.IdentityProviderId, opt => opt.MapFrom(src => src.User.IdentityProviderId))
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.User.ExternalId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.User.Avatar));

        CreateMap<PoolWriteRequest, Pool>();
    }
}
