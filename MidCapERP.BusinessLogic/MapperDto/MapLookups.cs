using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Lookups;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapLookups : Profile
    {
        public MapLookups()
        {
            CreateMap<Lookups, LookupsResponseDto>().ReverseMap();
            CreateMap<Lookups, LookupsRequestDto>().ReverseMap();
        }
    }
}