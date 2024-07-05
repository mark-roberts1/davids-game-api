using AutoMapper;
using Davids.Game.Data;
using Davids.Game.Models.Venues;

namespace Davids.Game.Venues;

public class VenueMappingProfile : Profile
{
    public VenueMappingProfile() : base()
    {
        CreateMap<Venue, VenueResponse>()
            .ForMember(dest => dest.Surface, opt => opt.MapFrom(src => (SurfaceType?)src.Surface));

        CreateMap<VenueWriteRequest, Venue>()
            .ForMember(dest => dest.Surface, opt => opt.MapFrom(src => (short?)src.Surface));
    }
}
