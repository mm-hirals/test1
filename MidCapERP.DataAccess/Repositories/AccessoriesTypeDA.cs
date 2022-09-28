using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;

namespace MidCapERP.DataAccess.Repositories
{
    public class AccessoriesTypeDA : IAccessoriesTypeDA
    {
        private readonly ISqlRepository<AccessoriesType> _accessoriesTypes;
        private readonly CurrentUser _currentUser;

        public AccessoriesTypeDA(ISqlRepository<AccessoriesType> accessoriesTypes, CurrentUser currentUser)
        {
            _accessoriesTypes = accessoriesTypes;
            _currentUser = currentUser;
        }

        public async Task<IQueryable<AccessoriesType>> GetAll(CancellationToken cancellationToken)
        {
            return await _accessoriesTypes.GetAsync(cancellationToken, x => x.IsDeleted == false && x.TenantId == _currentUser.TenantId);
        }

        public async Task<AccessoriesType> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _accessoriesTypes.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<AccessoriesType> CreateAccessoriesType(AccessoriesType model, CancellationToken cancellationToken)
        {
            return await _accessoriesTypes.InsertAsync(model, cancellationToken);
        }

        public async Task<AccessoriesType> UpdateAccessoriesType(int Id, AccessoriesType model, CancellationToken cancellationToken)
        {
            return await _accessoriesTypes.UpdateAsync(model, cancellationToken);
        }

        public async Task<AccessoriesType> DeleteAccessoriesType(int Id, CancellationToken cancellationToken)
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