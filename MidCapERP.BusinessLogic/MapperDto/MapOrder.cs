using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Order;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapOrder : Profile
    {
        public MapOrder()
        {
            CreateMap<Order, OrderApiRequestDto>().ReverseMap();
            CreateMap<Order, OrderApiResponseDto>().ReverseMap();
            CreateMap<OrderApiRequestDto, OrderApiResponseDto>().ReverseMap();
            CreateMap<Order, OrderResponseDto>().ReverseMap();
            CreateMap<OrderApiRequestDto, OrderApiResponseDto>().ReverseMap();
        }
    }
}