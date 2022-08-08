using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Accessories;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapAccessories : Profile
    {
        public MapAccessories()
        {
            CreateMap<Accessories, AccessoriesResponseDto>().ReverseMap();
            CreateMap<Accessories, AccessoriesRequestDto>().ReverseMap();
        }
    }
}