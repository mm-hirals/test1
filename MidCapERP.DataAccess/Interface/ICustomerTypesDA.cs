using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface ICustomerTypesDA
    {
        public Task<IQueryable<CustomerTypes>> GetAll(CancellationToken cancellationToken);

        public Task<CustomerTypes> GetById(Int64 Id, CancellationToken cancellationToken);
    }
}