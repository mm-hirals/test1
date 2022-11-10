using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class WrkImportCustomersDA : IWrkImportCustomersDA
    {
        private readonly ISqlRepository<WrkImportCustomers> _wrkImportCustomers;

        public WrkImportCustomersDA(ISqlRepository<WrkImportCustomers> wrkImportCustomers)
        {
            _wrkImportCustomers = wrkImportCustomers;
        }

        public async Task<WrkImportCustomers> Create(WrkImportCustomers model, CancellationToken cancellationToken)
        {
            return await _wrkImportCustomers.InsertAsync(model, cancellationToken);
        }

        public async Task<IQueryable<WrkImportCustomers>> GetAll(CancellationToken cancellationToken)
        {
            return await _wrkImportCustomers.GetAsync(cancellationToken);
        }

        public async Task<WrkImportCustomers> GetById(long Id, CancellationToken cancellationToken)
        {
            return await _wrkImportCustomers.GetByIdAsync(Id, cancellationToken);
        }
    }
}