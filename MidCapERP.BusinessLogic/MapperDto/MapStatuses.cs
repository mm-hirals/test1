using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Status;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapStatus : Profile
    {
        public MapStatus()
        {
            CreateMap<Statuses, StatusResponseDto>().ReverseMap();
            CreateMap<Statuses, StatusRequestDto>().ReverseMap();
        }
    }
}