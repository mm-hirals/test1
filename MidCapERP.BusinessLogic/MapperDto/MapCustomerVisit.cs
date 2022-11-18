using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.CustomerVisit;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapCustomerVisit : Profile
    {
        public MapCustomerVisit()
        {
            CreateMap<CustomerVisits, CustomerVisitRequestDto>().ReverseMap();
            CreateMap<CustomerVisits, CustomerVisitResponseDto>().ReverseMap();
        }
    }
}