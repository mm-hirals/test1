using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface ICustomerAddressesDA
    {
        public Task<IQueryable<CustomerAddresses>> GetAll(CancellationToken cancellationToken);

        public Task<CustomerAddresses> GetById(Int64 Id, CancellationToken cancellationToken);

        public Task<CustomerAddresses> CreateCustomerAddress(CustomerAddresses model, CancellationToken cancellationToken);

        public Task<CustomerAddresses> UpdateCustomerAddress(Int64 Id, CustomerAddresses model, CancellationToken cancellationToken);

        public Task<CustomerAddresses> DeleteCustomerAddress(Int64 Id, CancellationToken cancellationToken);
    }
}