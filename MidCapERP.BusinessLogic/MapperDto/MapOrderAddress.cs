using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Order;
using MidCapERP.Dto.OrderAddressesApi;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapOrderAddress : Profile
    {
        public MapOrderAddress()
        {
            CreateMap<OrderAddress, OrderAddressesApiRequestDto>().ReverseMap();
            CreateMap<OrderAddress, OrderAddressesApiResponseDto>().ReverseMap();
            CreateMap<OrderAddress, OrderAddressesResponseDto>().ReverseMap();
        }
    }
}