using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.WoodType;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapWoodType : Profile
    {
        public MapWoodType()
        {
            CreateMap<LookupValues, WoodTypeRequestDto>().ReverseMap();
            CreateMap<LookupValues, WoodTypeResponseDto>().ReverseMap();
        }
    }
}
