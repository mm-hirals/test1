using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface ICustomerAddressesDA
    {
        public Task<IQueryable<CustomerAddresses>> GetAll(CancellationToken cancellationToken);

        public Task<CustomerAddresses> GetById(int Id, CancellationToken cancellationToken);
    }
}