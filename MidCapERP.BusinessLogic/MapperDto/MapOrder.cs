using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Order;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapOrder : Profile
    {
        public MapOrder()
        {
            CreateMap<Order, OrderRequestDto>().ReverseMap();
            CreateMap<Order, OrderResponseDto>().ReverseMap();
        }
    }
}