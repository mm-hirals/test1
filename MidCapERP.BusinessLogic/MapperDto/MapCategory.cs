using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Category; 

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapCategory : Profile
    {
        public MapCategory()
        {
            CreateMap<Categories, CategoryRequestDto>().ReverseMap();
            CreateMap<Categories, CategoryResponseDto>().ReverseMap();
        }
    }
}