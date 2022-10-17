using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface ITenantSMTPDetailDA
    {
        public Task<TenantSMTPDetail> TenantSMTPDetailGetById(long Id, CancellationToken cancellationToken);

        public Task<TenantSMTPDetail> UpdateTenantSMTPDetail(long Id, TenantSMTPDetail model, CancellationToken cancellationToken);
    }
}