using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Customers;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapCustomers : Profile
    {
        public MapCustomers()
        {
            CreateMap<Customers, CustomersRequestDto>().ReverseMap();
            CreateMap<Customers, CustomersResponseDto>().ReverseMap();
        }
    }
}
