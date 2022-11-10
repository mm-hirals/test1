using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;

namespace MidCapERP.DataAccess.Repositories
{
    public class TenantSMTPDetailDA : ITenantSMTPDetailDA
    {
        private readonly ISqlRepository<TenantSMTPDetail> _tenantSMTPDetail;
        private readonly CurrentUser _currentUser;

        public TenantSMTPDetailDA(ISqlRepository<TenantSMTPDetail> tenantSMTPDetail, CurrentUser currentUser)
        {
            _tenantSMTPDetail = tenantSMTPDetail;
            _currentUser = currentUser;
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