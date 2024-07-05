using AutoMapper;
using Davids.Game.Data;
using Davids.Game.Models.Countries;

namespace Davids.Game.Countries;

public class CountryMappingProfile : Profile
{
    public CountryMappingProfile() : base()
    {
        CreateMap<Country, CountryResponse>();
        CreateMap<CountryWriteRequest, Country>();
    }
}
