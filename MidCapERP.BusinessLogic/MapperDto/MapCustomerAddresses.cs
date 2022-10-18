using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.CustomerAddresses;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapCustomerAddresses : Profile
    {
        public MapCustomerAddresses()
        {
            CreateMap<CustomerAddresses, CustomerAddressesRequestDto>().ReverseMap();
            CreateMap<CustomerAddresses, CustomerAddressesResponseDto>().ReverseMap();
            CreateMap<CustomerAddresses, CustomerAddressesApiRequestDto>().ReverseMap();
        }
    }
}