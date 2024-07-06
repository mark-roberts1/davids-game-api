using AutoMapper;
using Davids.Game.Models.Auth;
using Davids.Game.Models.Users;

namespace Davids.Game.Api.DiscordAuth;

public class DiscordMappingProfile : Profile
{
    public DiscordMappingProfile() : base()
    {
        CreateMap<DiscordTokenResponse, TokenResponse>();
    }
}
