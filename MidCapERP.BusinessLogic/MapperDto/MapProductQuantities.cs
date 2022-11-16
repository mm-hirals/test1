using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.ProductQuantities;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapProductQuantities : Profile
    {
        public MapProductQuantities()
        {
            CreateMap<ProductQuantities, ProductQuantitiesRequestDto>().ReverseMap();
            CreateMap<ProductQuantities, ProductQuantitiesResponseDto>().ReverseMap();
        }
    }
}