using MidCapERP.Dto.Tenant;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface ITenantBL
    {
        public Task<IEnumerable<TenantResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<TenantRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<TenantRequestDto> UpdateTenant(int Id, TenantRequestDto model, CancellationToken cancellationToken);
    }
}