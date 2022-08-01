using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class AccessoriesTypesDA : IAccessoriesTypesDA
    {
        private readonly ISqlRepository<AccessoriesTypes> _accessoriesTypes;

        public AccessoriesTypesDA(ISqlRepository<AccessoriesTypes> accessoriesTypes)
        {
            _accessoriesTypes = accessoriesTypes;
        }

        public async Task<IQueryable<AccessoriesTypes>> GetAll(CancellationToken cancellationToken)
        {
            return await _accessoriesTypes.GetAsync(cancellationToken, x => x.IsDeleted == false);
        }

        public async Task<AccessoriesTypes> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _accessoriesTypes.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<AccessoriesTypes> CreateAccessoriesTypes(AccessoriesTypes model, CancellationToken cancellationToken)
        {
            return await _accessoriesTypes.InsertAsync(model, cancellationToken);
        }

        public async Task<AccessoriesTypes> UpdateAccessoriesTypes(int Id, AccessoriesTypes model, CancellationToken cancellationToken)
        {
            return await _accessoriesTypes.UpdateAsync(model, cancellationToken);
        }

        public async Task<AccessoriesTypes> DeleteAccessoriesTypes(int Id, CancellationToken cancellationToken)
        {
            var entity = await _accessoriesTypes.GetByIdAsync(Id, cancellationToken);
            if (entity != null)
            {
                return await _accessoriesTypes.UpdateAsync(entity, cancellationToken);
            }
            return entity;
        }
    }
}