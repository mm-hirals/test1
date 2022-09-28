using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.ProductImage;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapProductImage : Profile
    {
        public MapProductImage()
        {
            CreateMap<ProductImage, ProductImageRequestDto>().ReverseMap();
            CreateMap<ProductImage, ProductImageResponseDto>().ReverseMap();
        }
    }
}