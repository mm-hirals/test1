using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.TenantBankDetail;

namespace MidCapERP.BusinessLogic.MapperDto
{
	public class MapTenantBankDetail : Profile
	{
		public MapTenantBankDetail()
		{
			CreateMap<TenantBankDetail, TenantBankDetailRequestDto>().ReverseMap();
			CreateMap<TenantBankDetail, TenantBankDetailResponseDto>().ReverseMap();
		}
	}
}