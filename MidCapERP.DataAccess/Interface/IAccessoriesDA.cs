using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IAccessoriesDA
    {
        public Task<IQueryable<Accessories>> GetAll(CancellationToken cancellationToken);

        public Task<Accessories> GetById(int Id, CancellationToken cancellationToken);

        public Task<Accessories> CreateAccessories(Accessories model, CancellationToken cancellationToken);

        public Task<Accessories> UpdateAccessories(int Id, Accessories model, CancellationToken cancellationToken);

        public Task<Accessories> DeleteAccessories(int Id, CancellationToken cancellationToken);
    }
}




