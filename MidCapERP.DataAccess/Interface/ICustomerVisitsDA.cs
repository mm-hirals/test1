using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface ICustomerVisitsDA
    {
        public Task<CustomerVisits> CreateCustomerVisits(CustomerVisits model, CancellationToken cancellationToken);

        public Task<IQueryable<CustomerVisits>> GetAll(CancellationToken cancellationToken);
    }
}