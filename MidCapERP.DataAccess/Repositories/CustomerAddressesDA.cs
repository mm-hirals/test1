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

        public async Task<CustomerAddresses> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _customerAddresses.GetByIdAsync(Id, cancellationToken);
        }
    }
}