using AutoMapper;
using Davids.Game.Data;
using Davids.Game.Models.Leagues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Davids.Game.Leagues;
public class LeagueMappingProfile : Profile
{
    public LeagueMappingProfile() : base()
    {
        CreateMap<League, LeagueResponse>();
    }
}
