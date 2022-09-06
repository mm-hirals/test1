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

        public async Task<IQueryable<Customers>> GetAll(CancellationToken cancellationToken)
        {
            return await _customers.GetAsync(cancellationToken, x => x.IsDeleted == false);
        }

        public async Task<Customers> GetById(Int64 Id, CancellationToken cancellationToken)
        {
            return await _customers.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<Customers> CreateCustomers(Customers model, CancellationToken cancellationToken)
        {
            return await _customers.InsertAsync(model, cancellationToken);
        }

        public async Task<Customers> UpdateCustomers(Int64 Id, Customers model, CancellationToken cancellationToken)
        {
            return await _customers.UpdateAsync(model, cancellationToken);
        }
    }
}