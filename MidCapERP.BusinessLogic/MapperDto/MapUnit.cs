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

            //CreateMap<LookupValues, UnitResponseDto>()
            //   .ForMember(d => d.LookupValueName, opt => opt.MapFrom(s => s.LookupValueName))
            //   .ForMember(d => d.LookupValueId, opt => opt.MapFrom(s => s.LookupValueId))
            //   .ForMember(d => d.LookupId, opt => opt.MapFrom(s => s.LookupId));

            //CreateMap<Lookups, UnitResponseDto>()
            //    .ForMember(d => d.LookupName, opt => opt.MapFrom(s => s.LookupName));
        }
    }
}