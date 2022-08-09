using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Wood;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapWood : Profile
    {
        public MapWood()
        {
            CreateMap<Wood, WoodRequestDto>().ReverseMap();
            CreateMap<Wood, WoodResponseDto>().ReverseMap();
        }
    }
}