using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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