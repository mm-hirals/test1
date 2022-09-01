using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.UserTenantMapping;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapUserTenantMapping : Profile
    {
        public MapUserTenantMapping()
        {
            CreateMap<UserTenantMapping, UserTenantMappingRequestDto>().ReverseMap();
            CreateMap<UserTenantMapping, UserTenantMappingResponseDto>().ReverseMap();
        }
    }
}