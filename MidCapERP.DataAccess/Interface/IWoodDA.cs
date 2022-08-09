using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IWoodDA
    {
        public Task<IQueryable<Wood>> GetAll(CancellationToken cancellationToken);

        public Task<Wood> GetById(int Id, CancellationToken cancellationToken);

        public Task<Wood> CreateWood(Wood model, CancellationToken cancellationToken);

        public Task<Wood> UpdateWood(int Id, Wood model, CancellationToken cancellationToken);

        public Task<Wood> DeleteWood(int Id, CancellationToken cancellationToken);
    }
}