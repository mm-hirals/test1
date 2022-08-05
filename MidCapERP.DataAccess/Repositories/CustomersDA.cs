using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class CustomersDA : ICustomersDA
    {
        private readonly ISqlRepository<Customers> _customers;

        public CustomersDA(ISqlRepository<Customers> customers)
        {
            _customers = customers;
        }

        public async Task<IQueryable<Customers>> GetAllCustomers(CancellationToken cancellationToken)
        {
            return await _customers.GetAsync(cancellationToken, x => x.IsDeleted == false);
        }

        public async Task<Customers> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _customers.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<Customers> CreateCustomers(Customers model, CancellationToken cancellationToken)
        {
            return await _customers.InsertAsync(model, cancellationToken);
        }

        public async Task<Customers> UpdateCustomers(int Id, Customers model, CancellationToken cancellationToken)
        {
            return await _customers.UpdateAsync(model, cancellationToken);
        }

        public async Task<Customers> DeleteCustomers(int Id, CancellationToken cancellationToken)
        {
            var entity = await _customers.GetByIdAsync(Id, cancellationToken);
            if (entity != null)
            {
                return await _customers.UpdateAsync(entity, cancellationToken);
            }
            return entity;
        }
    }
}