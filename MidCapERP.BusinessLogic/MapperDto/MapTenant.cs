using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Tenant;
using AutoMapper;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapTenant : Profile
    {
        public MapTenant()
        {
            CreateMap<Tenant, TenantResponseDto>().ReverseMap();
            CreateMap<Tenant, TenantRequestDto>().ReverseMap();
        }
    }
}
