using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.CustomersTypes;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapCustomerTypes : Profile
    {
        public MapCustomerTypes()
        {
            CreateMap<CustomerTypes, CustomersTypesResponseDto>().ReverseMap();
        }
    }
}