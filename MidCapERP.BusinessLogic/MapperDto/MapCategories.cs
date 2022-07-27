using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Categories;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapCategories : Profile
    {
        public MapCategories()
        {
            CreateMap<Categories, CategoriesResponseDto>().ReverseMap();
            CreateMap<Categories, CategoriesRequestDto>().ReverseMap();
        }
    }
}