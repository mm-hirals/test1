using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class TenantBankDetailDA : ITenantBankDetailDA
    {
        private readonly ISqlRepository<TenantBankDetail> _tenantBankDetail;

        public TenantBankDetailDA(ISqlRepository<TenantBankDetail> tenantBankDetail)
        {
            _tenantBankDetail = tenantBankDetail;
        }

        public async Task<IQueryable<TenantBankDetail>> GetAll(CancellationToken cancellationToken)
        {
            return await _tenantBankDetail.GetAsync(cancellationToken, x => x.IsDeleted == false);
        }

        public async Task<TenantBankDetail> GetById(int Id, CancellationToken cancellationToken)
        {
			try
			{
                return await _tenantBankDetail.GetByIdAsync(Convert.ToInt64(Id), cancellationToken);
            }
			catch (Exception e)
			{

				throw;
			}
        }

        public async Task<TenantBankDetail> UpdateTenantBankDetail(int Id, TenantBankDetail model, CancellationToken cancellationToken)
        {
            return await _tenantBankDetail.UpdateAsync(model, cancellationToken);
        }
    }
}
