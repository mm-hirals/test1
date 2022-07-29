using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Company;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapCompany : Profile
    {
        public MapCompany()
        {
            CreateMap<LookupValues, CompanyRequestDto>().ReverseMap();
            CreateMap<LookupValues, CompanyResponseDto>().ReverseMap();

            //CreateMap<LookupValues, CompanyResponseDto>()
            //   .ForMember(d => d.LookupValueName, opt => opt.MapFrom(s => s.LookupValueName))
            //   .ForMember(d => d.LookupValueId, opt => opt.MapFrom(s => s.LookupValueId))
            //   .ForMember(d => d.LookupId, opt => opt.MapFrom(s => s.LookupId));

            //CreateMap<Lookups, CompanyResponseDto>()
            //    .ForMember(d => d.LookupName, opt => opt.MapFrom(s => s.LookupName));
        }
    }
}
