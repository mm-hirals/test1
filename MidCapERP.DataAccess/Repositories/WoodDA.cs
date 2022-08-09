using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class WoodDA : IWoodDA
    {
        private readonly ISqlRepository<Woods> _wood;

        public WoodDA(ISqlRepository<Woods> wood)
        {
            _wood = wood;
        }

        public async Task<IQueryable<Woods>> GetAll(CancellationToken cancellationToken)
        {
            return await _wood.GetAsync(cancellationToken, x => x.IsDeleted == false);
        }

        public async Task<Woods> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _wood.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<Woods> CreateWood(Woods model, CancellationToken cancellationToken)
        {
            return await _wood.InsertAsync(model, cancellationToken);
        }

        public async Task<Woods> UpdateWood(int Id, Woods model, CancellationToken cancellationToken)
        {
            return await _wood.UpdateAsync(model, cancellationToken);
        }

        public async Task<Woods> DeleteWood(int Id, CancellationToken cancellationToken)
        {
            var entity = await _wood.GetByIdAsync(Id, cancellationToken);
            if (entity != null)
            {
                return await _wood.UpdateAsync(entity, cancellationToken);
            }
            return entity;
        }
    }
}