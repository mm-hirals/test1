using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.TenantBankDetail;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface ITenantBankDetailBL
    {
        public Task<IEnumerable<TenantBankDetailResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<TenantBankDetailResponseDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<JsonRepsonse<TenantBankDetailResponseDto>> GetFilterTenantBankDetailData(TenantBankDetailDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);
        public Task<TenantBankDetailRequestDto> CreateTenantBankDetail(TenantBankDetailRequestDto model, CancellationToken cancellationToken);
        public Task<TenantBankDetailRequestDto> UpdateTenantBankDetail(int Id, TenantBankDetailRequestDto model, CancellationToken cancellationToken);
        public Task<TenantBankDetailRequestDto> DeleteTenantBankDetail(int Id, TenantBankDetailRequestDto model, CancellationToken cancellationToken);
    }
}