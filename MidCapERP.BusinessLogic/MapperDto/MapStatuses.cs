using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Statuses;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapStatuses : Profile
    {
        public MapStatuses()
        {
            CreateMap<Statuses, StatusesResponseDto>().ReverseMap();
            CreateMap<Statuses, StatusesRequestDto>().ReverseMap();
        }
    }
}