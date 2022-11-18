using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;

namespace MidCapERP.DataAccess.Repositories
{
    public class CustomersDA : ICustomersDA
    {
        private ISqlRepository<Customers> _customers;
        private CurrentUser _currentUser;

        public CustomersDA(ISqlRepository<Customers> customers, CurrentUser currentUser)
        {
            _customers = customers;
            _currentUser = currentUser;
        }

        public async Task CreateScope(CurrentUser currentUser, CancellationToken cancellationToken)
        {
            _currentUser = currentUser;
        }

        public async Task<IQueryable<Customers>> GetAll(CancellationToken cancellationToken)
        {
            return await _customers.GetAsync(cancellationToken, x => x.IsDeleted == false && x.TenantId == _currentUser.TenantId);
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