using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.ErrorLogs;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapErrorLogs : Profile
    {
        public MapErrorLogs()
        {
            CreateMap<ErrorLogs, ErrorLogsResponseDto>().ReverseMap();
            CreateMap<ErrorLogs, ErrorLogsRequestDto>().ReverseMap();
        }
    }
}