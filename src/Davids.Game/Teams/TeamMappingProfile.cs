using AutoMapper;
using Davids.Game.Data;
using Davids.Game.Models.Teams;

namespace Davids.Game.Teams;

public class TeamMappingProfile : Profile
{
    public TeamMappingProfile() : base()
    {
        CreateMap<Team, TeamResponse>();
        CreateMap<TeamWriteRequest, Team>();
    }
}
