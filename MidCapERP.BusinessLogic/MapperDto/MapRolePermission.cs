using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.RolePermission;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapRolePermission : Profile
    {
        public MapRolePermission()
        {
            CreateMap<ApplicationRole, RolePermissionRequestDto>().ReverseMap();
            CreateMap<ApplicationRole, RolePermissionResponseDto>().ReverseMap();
        }
    }
}