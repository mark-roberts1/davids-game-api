using AutoMapper;
using Davids.Game.Data;
using Davids.Game.Models.Statistics;

namespace Davids.Game.Statistics;

public class StatisticMappingProfile : Profile
{
    public StatisticMappingProfile() : base()
    {
        CreateMap<Statistic, StatisticResponse>();

        CreateMap<StatisticWriteRequest, Statistic>();
    }
}
