using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IFabricDA
    {
        public Task<IQueryable<Fabric>> GetAll(CancellationToken cancellationToken);

        public Task<Fabric> GetById(int Id, CancellationToken cancellationToken);

        public Task<Fabric> CreateFabric(Fabric model, CancellationToken cancellationToken);

        public Task<Fabric> UpdateFabric(int Id, Fabric model, CancellationToken cancellationToken);

        public Task<Fabric> DeleteFabric(int Id, CancellationToken cancellationToken);
    }
}