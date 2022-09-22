using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface ITenantBankDetailDA
    {
        public Task<IQueryable<TenantBankDetail>> GetAll(CancellationToken cancellationToken);

        public Task<TenantBankDetail> GetById(int Id, CancellationToken cancellationToken);

        public Task<TenantBankDetail> UpdateTenantBankDetail(int Id, TenantBankDetail model, CancellationToken cancellationToken);
    }
}