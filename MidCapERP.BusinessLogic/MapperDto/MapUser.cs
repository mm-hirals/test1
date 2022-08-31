using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.User;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapUser : Profile
    {
        public MapUser()
        {
            CreateMap<ApplicationUser, UserRequestDto>().ReverseMap();
            CreateMap<ApplicationUser, UserResponseDto>().ReverseMap();
        }
    }
}