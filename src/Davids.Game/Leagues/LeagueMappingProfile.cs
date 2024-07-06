using AutoMapper;
using Davids.Game.Data;
using Davids.Game.Models.Leagues;

namespace Davids.Game.Leagues;
public class LeagueMappingProfile : Profile
{
    public LeagueMappingProfile() : base()
    {
        CreateMap<League, LeagueResponse>();

        CreateMap<TeamSeasonLeague, LeagueSeasonResponse>()
            .ForMember(dest => dest.Season, opt => opt.MapFrom(src => src.Season))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.League.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.League.Name))
            .ForMember(dest => dest.LeagueTypeId, opt => opt.MapFrom(src => src.League.LeagueTypeId))
            .ForMember(dest => dest.LogoLink, opt => opt.MapFrom(src => src.League.LogoLink))
            .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.League.CountryId))
            .ForMember(dest => dest.SourceId, opt => opt.MapFrom(src => src.League.SourceId))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.League.Country));

        CreateMap<LeagueWriteRequest, League>();
    }
}
