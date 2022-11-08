using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.InteriorAddresses;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapInteriorAddresses : Profile
    {
        public MapInteriorAddresses()
        {
            CreateMap<CustomerAddresses, InteriorAddressesRequestDto>().ReverseMap();
            CreateMap<CustomerAddresses, InteriorAddressesResponseDto>().ReverseMap();
        }
    }
}