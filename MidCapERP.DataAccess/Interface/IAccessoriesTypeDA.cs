using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IAccessoriesTypeDA
    {
        public Task<IQueryable<AccessoriesType>> GetAll(CancellationToken cancellationToken);

        public Task<AccessoriesType> GetById(int Id, CancellationToken cancellationToken);

        public Task<AccessoriesType> CreateAccessoriesType(AccessoriesType model, CancellationToken cancellationToken);

        public Task<AccessoriesType> UpdateAccessoriesType(int Id, AccessoriesType model, CancellationToken cancellationToken);

        public Task<AccessoriesType> DeleteAccessoriesType(int Id, CancellationToken cancellationToken);
    }
}