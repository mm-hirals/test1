using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IFabricDA
    {
        public Task<IQueryable<Fabrics>> GetAll(CancellationToken cancellationToken);

        public Task<Fabrics> GetById(int Id, CancellationToken cancellationToken);

        public Task<Fabrics> CreateFabric(Fabrics model, CancellationToken cancellationToken);

        public Task<Fabrics> UpdateFabric(int Id, Fabrics model, CancellationToken cancellationToken);

        public Task<Fabrics> DeleteFabric(int Id, CancellationToken cancellationToken);
    }
}