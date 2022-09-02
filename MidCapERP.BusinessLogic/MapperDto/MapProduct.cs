using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Product;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapProduct : Profile
    {
        public MapProduct()
        {
            CreateMap<Product, ProductRequestDto>().ReverseMap();
            CreateMap<Product, ProductResponseDto>().ReverseMap();
        }
    }
}