using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.ProductMaterial;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapProductMaterial : Profile
    {
        public MapProductMaterial()
        {
            CreateMap<ProductMaterial, ProductMaterialRequestDto>().ReverseMap();
            CreateMap<ProductMaterial, ProductMaterialsResponseDto>().ReverseMap();
        }
    }
}