using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;

namespace MidCapERP.DataAccess.Interface
{
    public interface ICustomersDA
    {
        public Task<IQueryable<Customers>> GetAll(CancellationToken cancellationToken);

        public Task CreateScope(CurrentUser currentUser, CancellationToken cancellationToken);

        public Task<Customers> GetById(Int64 Id, CancellationToken cancellationToken);

        public Task<Customers> CreateCustomers(Customers model, CancellationToken cancellationToken);

        public Task<Customers> UpdateCustomers(Int64 Id, Customers model, CancellationToken cancellationToken);

        public Task<CustomerVisits> CreateCustomerVisits(CustomerVisits model, CancellationToken cancellationToken);
    }
}