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
        }
    }
}
