using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Category; 

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapCategory : Profile
    {
        public MapCategory()
        {
            CreateMap<LookupValues, CategoryRequestDto>().ReverseMap();
            CreateMap<LookupValues, CategoryResponseDto>().ReverseMap();
        }
    }
}