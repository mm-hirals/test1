using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.ArchitectAddresses;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapArchitectAddresses : Profile
    {
        public MapArchitectAddresses()
        {
            CreateMap<CustomerAddresses, ArchitectAddressesRequestDto>().ReverseMap();
            CreateMap<CustomerAddresses, ArchitectAddressesResponseDto>().ReverseMap();
        }
    }
}