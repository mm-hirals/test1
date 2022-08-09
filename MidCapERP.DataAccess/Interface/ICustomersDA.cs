using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface ICustomersDA
    {
        public Task<IQueryable<Customers>> GetAll(CancellationToken cancellationToken);

        public Task<Customers> GetById(int Id, CancellationToken cancellationToken);

        public Task<Customers> CreateCustomers(Customers model, CancellationToken cancellationToken);

        public Task<Customers> UpdateCustomers(int Id, Customers model, CancellationToken cancellationToken);

        public Task<Customers> DeleteCustomers(int Id, CancellationToken cancellationToken);
    }
}