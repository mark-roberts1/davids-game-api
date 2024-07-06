using AutoMapper;
using Davids.Game.Data;
using Davids.Game.Models.Venues;

namespace Davids.Game.Venues;

public class VenueMappingProfile : Profile
{
    public VenueMappingProfile() : base()
    {
        CreateMap<Venue, VenueResponse>();

        CreateMap<VenueWriteRequest, Venue>();
    }
}
