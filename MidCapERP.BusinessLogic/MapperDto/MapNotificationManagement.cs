using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.NotificationManagement;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapNotificationManagement : Profile
    {
        public MapNotificationManagement()
        {
            CreateMap<NotificationManagement, NotificationManagementRequestDto>().ReverseMap();
            CreateMap<NotificationManagement, NotificationManagementResponseDto>().ReverseMap();
        }
    }
}