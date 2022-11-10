using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IWrkImportCustomersDA
    {
        public Task<IQueryable<WrkImportCustomers>> GetAll(CancellationToken cancellationToken);

        public Task<WrkImportCustomers> GetById(Int64 Id, CancellationToken cancellationToken);

        public Task<WrkImportCustomers> Create(WrkImportCustomers model, CancellationToken cancellationToken);
    }
}