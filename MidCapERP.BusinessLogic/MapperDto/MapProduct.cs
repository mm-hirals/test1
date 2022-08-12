using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Product;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapProduct : Profile
    {
        public MapProduct()
        {
            CreateMap<Products, ProductRequestDto>().ReverseMap();
            CreateMap<Woods, ProductResponseDto>().ReverseMap();
        }
    }
}