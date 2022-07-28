using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Category;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapCategory : Profile
    {
        public MapCategory()
        {
            CreateMap<LookupValues, CategoryRequestDto>().ReverseMap();
            CreateMap<LookupValues, CategoryResponseDto>().ReverseMap();

            //CreateMap<LookupValues, CategoryResponseDto>()
            //   .ForMember(d => d.LookupValueName, opt => opt.MapFrom(s => s.LookupValueName))
            //   .ForMember(d => d.LookupValueId, opt => opt.MapFrom(s => s.LookupValueId))
            //   .ForMember(d => d.LookupId, opt => opt.MapFrom(s => s.LookupId));

            //CreateMap<Lookups, CategoryResponseDto>()
            //    .ForMember(d => d.LookupName, opt => opt.MapFrom(s => s.LookupName));
        }
    }
}