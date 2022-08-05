using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IAccessoriesTypesDA
    {
        public Task<IQueryable<AccessoriesTypes>> GetAll(CancellationToken cancellationToken);

        public Task<AccessoriesTypes> GetById(int Id, CancellationToken cancellationToken);

        public Task<AccessoriesTypes> CreateAccessoriesTypes(AccessoriesTypes model, CancellationToken cancellationToken);

        public Task<AccessoriesTypes> UpdateAccessoriesTypes(int Id, AccessoriesTypes model, CancellationToken cancellationToken);

        public Task<AccessoriesTypes> DeleteAccessoriesTypes(int Id, CancellationToken cancellationToken);
    }
}
