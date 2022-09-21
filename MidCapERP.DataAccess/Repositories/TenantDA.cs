using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class TenantDA : ITenantDA
    {
        private readonly ISqlRepository<Tenant> _tenant;

        public TenantDA(ISqlRepository<Tenant> tenant)
        {
            _tenant = tenant;
        }

        public async Task<IQueryable<Tenant>> GetAll(CancellationToken cancellationToken)
        {
            return await _tenant.GetAsync(cancellationToken, x => x.IsDeleted == false);
        }

        public async Task<Tenant> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _tenant.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<Tenant> UpdateTenant(int Id, Tenant model, CancellationToken cancellationToken)
        {
            return await _tenant.UpdateAsync(model, cancellationToken);
        }
    }
}