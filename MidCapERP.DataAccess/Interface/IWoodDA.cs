using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IWoodDA
    {
        public Task<IQueryable<Woods>> GetAll(CancellationToken cancellationToken);

        public Task<Woods> GetById(int Id, CancellationToken cancellationToken);

        public Task<Woods> CreateWood(Woods model, CancellationToken cancellationToken);

        public Task<Woods> UpdateWood(int Id, Woods model, CancellationToken cancellationToken);

        public Task<Woods> DeleteWood(int Id, CancellationToken cancellationToken);
    }
}