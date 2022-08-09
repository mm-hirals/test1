using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class WoodDA : IWoodDA
    {
        private readonly ISqlRepository<Wood> _wood;

        public WoodDA(ISqlRepository<Wood> wood)
        {
            _wood = wood;
        }

        public async Task<IQueryable<Wood>> GetAll(CancellationToken cancellationToken)
        {
            return await _wood.GetAsync(cancellationToken, x => x.IsDeleted == false);
        }

        public async Task<Wood> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _wood.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<Wood> CreateWood(Wood model, CancellationToken cancellationToken)
        {
            return await _wood.InsertAsync(model, cancellationToken);
        }

        public async Task<Wood> UpdateWood(int Id, Wood model, CancellationToken cancellationToken)
        {
            return await _wood.UpdateAsync(model, cancellationToken);
        }

        public async Task<Wood> DeleteWood(int Id, CancellationToken cancellationToken)
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