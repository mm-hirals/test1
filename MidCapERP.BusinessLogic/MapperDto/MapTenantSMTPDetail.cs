using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.TenantSMTPDetail;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapTenantSMTPDetail : Profile
    {
        public MapTenantSMTPDetail()
        {
            CreateMap<TenantSMTPDetail, TenantSMTPDetailRequestDto>().ReverseMap();
            CreateMap<TenantSMTPDetail, TenantSMTPDetailResponseDto>().ReverseMap();
        }
    }
}
