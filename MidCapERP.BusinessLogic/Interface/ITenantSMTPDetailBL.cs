using MidCapERP.Dto.TenantSMTPDetail;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface ITenantSMTPDetailBL
    {
        public Task<TenantSMTPDetailResponseDto> TenantSMTPDetailGetById(long Id, CancellationToken cancellationToken);

        public Task<TenantSMTPDetailRequestDto> UpdateTenantSMTPDetail(TenantSMTPDetailRequestDto model, CancellationToken cancellationToken);
    }
}