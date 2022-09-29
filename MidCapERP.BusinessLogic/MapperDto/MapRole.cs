using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Role;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapRole : Profile
    {
        public MapRole()
        {
            CreateMap<ApplicationRole, RoleRequestDto>().ReverseMap();
            CreateMap<ApplicationRole, RoleResponseDto>().ReverseMap();
            CreateMap<IdentityResult, RoleRequestDto>().ReverseMap();
        }
    }
}