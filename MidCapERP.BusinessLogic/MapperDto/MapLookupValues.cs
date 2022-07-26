using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.LookupValues;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapLookupValues : Profile
    {
        public MapLookupValues()
        {
            CreateMap<LookupValues, LookupValuesRequestDto>().ReverseMap();
            CreateMap<LookupValues, LookupValuesResponseDto>().ReverseMap();
        }
    }
}
