using MidCapERP.Dto.TenantBankDetail;

namespace MidCapERP.BusinessLogic.Interface
{
	public interface ITenantBankDetailBL
    {
        public Task<IEnumerable<TenantBankDetailResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<TenantBankDetailRequestDto> GetById(int id, CancellationToken cancellationToken);

        public Task<TenantBankDetailRequestDto> UpdateTenantBankDetail(int id, TenantBankDetailRequestDto model, CancellationToken cancellationToken);
       
    }
}