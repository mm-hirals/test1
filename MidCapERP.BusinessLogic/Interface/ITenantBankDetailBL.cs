using MidCapERP.Dto.TenantBankDetail;

namespace MidCapERP.BusinessLogic.Interface
{
	public interface ITenantBankDetailBL
    {
        public Task<IEnumerable<TenantBankDetailResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<TenantBankDetailRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<TenantBankDetailRequestDto> UpdateTenantBankDetail(int Id, TenantBankDetailRequestDto model, CancellationToken cancellationToken);
       
    }
}