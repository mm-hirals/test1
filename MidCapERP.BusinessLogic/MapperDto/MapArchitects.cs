using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Architect;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapArchitects : Profile
    {
        public MapArchitects()
        {
            CreateMap<Customers, ArchitectRequestDto>().ReverseMap();
            CreateMap<Customers, ArchitectResponseDto>().ReverseMap();
        }
    }
}