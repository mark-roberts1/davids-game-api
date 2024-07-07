using AutoMapper;
using Davids.Game.Data;
using Davids.Game.Models.UserPools;
using Davids.Game.Models.Users;

namespace Davids.Game.Users;

public class UserMappingProfile : Profile
{
    public UserMappingProfile() : base()
    {
        CreateMap<User, UserResponse>();
        CreateMap<UserWriteRequest, User>();

        CreateMap<UserPool, UserPoolResponse>()
            .ForMember(dest => dest.UserPoolId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Attributes, opt => opt.MapFrom(src => (UserPoolAttribute)src.Attributes))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Pool.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Pool.Name))
            .ForMember(dest => dest.LeagueId, opt => opt.MapFrom(src => src.Pool.LeagueId))
            .ForMember(dest => dest.Season, opt => opt.MapFrom(src => src.Pool.Season))
            .ForMember(dest => dest.DiscordServerId, opt => opt.MapFrom(src => src.Pool.DiscordServerId))
            .ForMember(dest => dest.JoinCode, opt => opt.MapFrom(src => src.Pool.JoinCode));
    }
}
