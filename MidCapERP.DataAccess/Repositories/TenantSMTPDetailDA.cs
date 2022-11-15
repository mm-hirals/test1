using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class TenantSMTPDetailDA : ITenantSMTPDetailDA
    {
        private readonly ISqlRepository<TenantSMTPDetail> _tenantSMTPDetail;

        public TenantSMTPDetailDA(ISqlRepository<TenantSMTPDetail> tenantSMTPDetail)
        {
            _tenantSMTPDetail = tenantSMTPDetail;
        }

        public async Task<IQueryable<TenantSMTPDetail>> GetAll(CancellationToken cancellationToken)
        {
            return await _tenantSMTPDetail.GetAsync(cancellationToken, x => x.IsDeleted == false);
        }

        public async Task<TenantSMTPDetail> TenantSMTPDetailGetById(long Id, CancellationToken cancellationToken)
        {
            return await _tenantSMTPDetail.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<TenantSMTPDetail> UpdateTenantSMTPDetail(long Id, TenantSMTPDetail model, CancellationToken cancellationToken)
        {
            return await _tenantSMTPDetail.UpdateAsync(model, cancellationToken);
        }
    }
}