using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.OrderSet;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapOrderSet : Profile
    {
        public MapOrderSet()
        {
            CreateMap<OrderSet, OrderSetRequestDto>().ReverseMap();
            CreateMap<OrderSet, OrderSetResponseDto>().ReverseMap();
        }
    }
}