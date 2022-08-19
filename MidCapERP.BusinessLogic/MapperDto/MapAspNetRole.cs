using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.AspNetRole;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapAspNetRole : Profile
    {
        public MapAspNetRole()
        {
            CreateMap<ApplicationRole, AspNetRoleResponseDto>().ReverseMap();
        }
    }
}