using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class AccessoriesDA : IAccessoriesDA
    {
        private readonly ISqlRepository<Accessories> _accessories;

        public AccessoriesDA(ISqlRepository<Accessories> accessories)
        {
            _accessories = accessories;
        }

        public async Task<IQueryable<Accessories>> GetAll(CancellationToken cancellationToken)
        {
            return await _accessories.GetAsync(cancellationToken, x => x.IsDeleted == false);
        }

        public async Task<Accessories> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _accessories.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<Accessories> CreateAccessories(Accessories model, CancellationToken cancellationToken)
        {
            return await _accessories.InsertAsync(model, cancellationToken);
        }

        public async Task<Accessories> UpdateAccessories(int Id, Accessories model, CancellationToken cancellationToken)
        {
            return await _accessories.UpdateAsync(model, cancellationToken);
        }

        public async Task<Accessories> DeleteAccessories(int Id, CancellationToken cancellationToken)
        {
            var entity = await _accessories.GetByIdAsync(Id, cancellationToken);
            if (entity != null)
            {
                return await _accessories.UpdateAsync(entity, cancellationToken);
            }
            return entity;
        }
    }
}