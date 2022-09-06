using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class CustomerAddressesDA : ICustomerAddressesDA
    {
        private readonly ISqlRepository<CustomerAddresses> _customerAddresses;

        public CustomerAddressesDA(ISqlRepository<CustomerAddresses> customerAddresses)
        {
            _customerAddresses = customerAddresses;
        }

        public async Task<IQueryable<CustomerAddresses>> GetAll(CancellationToken cancellationToken)
        {
            return await _customerAddresses.GetAsync(cancellationToken, x => x.IsDeleted == false);
        }

        public async Task<CustomerAddresses> GetById(Int64 Id, CancellationToken cancellationToken)
        {
            return await _customerAddresses.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<CustomerAddresses> CreateCustomerAddress(CustomerAddresses model, CancellationToken cancellationToken)
        {
            return await _customerAddresses.InsertAsync(model, cancellationToken);
        }

        public async Task<CustomerAddresses> UpdateCustomerAddress(Int64 Id, CustomerAddresses model, CancellationToken cancellationToken)
        {
            return await _customerAddresses.UpdateAsync(model, cancellationToken);
        }
        public async Task<CustomerAddresses> DeleteCustomerAddress(Int64 Id, CancellationToken cancellationToken)
        {
            var entity = await _customerAddresses.GetByIdAsync(Id, cancellationToken);
            if (entity != null)
            {
                return await _customerAddresses.UpdateAsync(entity, cancellationToken);
            }
            return entity;
        }
    }
}