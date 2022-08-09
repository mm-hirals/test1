using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class FabricDA : IFabricDA
    {
        private readonly ISqlRepository<Fabric> _fabric;

        public FabricDA(ISqlRepository<Fabric> fabric)
        {
            _fabric = fabric;
        }

        public async Task<IQueryable<Fabric>> GetAll(CancellationToken cancellationToken)
        {
            return await _fabric.GetAsync(cancellationToken, x => x.IsDeleted == false);
        }

        public async Task<Fabric> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _fabric.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<Fabric> CreateFabric(Fabric model, CancellationToken cancellationToken)
        {
            return await _fabric.InsertAsync(model, cancellationToken);
        }

        public async Task<Fabric> UpdateFabric(int Id, Fabric model, CancellationToken cancellationToken)
        {
            return await _fabric.UpdateAsync(model, cancellationToken);
        }

        public async Task<Fabric> DeleteFabric(int Id, CancellationToken cancellationToken)
        {
            var entity = await _fabric.GetByIdAsync(Id, cancellationToken);
            if (entity != null)
            {
                return await _fabric.UpdateAsync(entity, cancellationToken);
            }
            return entity;
        }
    }
}