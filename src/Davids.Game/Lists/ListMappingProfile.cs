using AutoMapper;
using Davids.Game.Data;
using Davids.Game.Models.Lists;

namespace Davids.Game.Lists;

public class ListMappingProfile : Profile
{
    public ListMappingProfile() : base()
    {
        CreateMap<ListEntry, ListEntryResponse>();
        CreateMap<List, ListResponse>();
        CreateMap<ListEntryWriteRequest, ListEntry>();
        CreateMap<ListWriteRequest, List>();
    }
}
