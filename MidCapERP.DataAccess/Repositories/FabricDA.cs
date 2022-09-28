using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;

namespace MidCapERP.DataAccess.Repositories
{
    public class FabricDA : IFabricDA
    {
        private readonly ISqlRepository<Fabrics> _fabric;
        private readonly CurrentUser _currentUser;

        public FabricDA(ISqlRepository<Fabrics> fabric, CurrentUser currentUser)
        {
            _fabric = fabric;
            _currentUser = currentUser;
        }

        public async Task<IQueryable<Fabrics>> GetAll(CancellationToken cancellationToken)
        {
            return await _fabric.GetAsync(cancellationToken, x => x.IsDeleted == false && x.TenantId == _currentUser.TenantId);
        }

        public async Task<Fabrics> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _fabric.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<Fabrics> CreateFabric(Fabrics model, CancellationToken cancellationToken)
        {
            return await _fabric.InsertAsync(model, cancellationToken);
        }

        public async Task<Fabrics> UpdateFabric(int Id, Fabrics model, CancellationToken cancellationToken)
        {
            return await _fabric.UpdateAsync(model, cancellationToken);
        }

        public async Task<Fabrics> DeleteFabric(int Id, CancellationToken cancellationToken)
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