using AutoMapper;
using Davids.Game.Data;
using Davids.Game.Models.Seasons;

namespace Davids.Game.Seasons;

public class SeasonMappingProfile : Profile
{
    public SeasonMappingProfile() : base()
    {
        CreateMap<Season, SeasonResponse>();
        CreateMap<SeasonWriteRequest, Season>();
    }
}
