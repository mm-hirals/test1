using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.OrderSetItem;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapOrderSetItem : Profile
    {
        public MapOrderSetItem()
        {
            CreateMap<OrderSetItem, OrderSetItemRequestDto>().ReverseMap();
            CreateMap<OrderSetItem, OrderSetItemResponseDto>().ReverseMap();
        }
    }
}