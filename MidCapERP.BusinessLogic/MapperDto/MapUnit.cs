using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Unit;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapUnit : Profile
    {
        public MapUnit()
        {
            CreateMap<LookupValues, UnitRequestDto>().ReverseMap();
            CreateMap<LookupValues, UnitResponseDto>().ReverseMap();
        }
    }
}