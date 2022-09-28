using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;

namespace MidCapERP.DataAccess.Repositories
{
    public class RawMaterialDA : IRawMaterialDA
    {
        private readonly ISqlRepository<RawMaterial> _rawMaterial;
        private readonly CurrentUser _currentUser;

        public RawMaterialDA(ISqlRepository<RawMaterial> rawMaterial, CurrentUser currentUser)
        {
            _rawMaterial = rawMaterial;
            _currentUser = currentUser;
        }

        public async Task<IQueryable<RawMaterial>> GetAll(CancellationToken cancellationToken)
        {
            return await _rawMaterial.GetAsync(cancellationToken, x => x.TenantId == _currentUser.TenantId && x.IsDeleted == false);
        }

        public async Task<RawMaterial> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _rawMaterial.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<RawMaterial> CreateRawMaterial(RawMaterial model, CancellationToken cancellationToken)
        {
            return await _rawMaterial.InsertAsync(model, cancellationToken);
        }

        public async Task<RawMaterial> UpdateRawMaterial(int Id, RawMaterial model, CancellationToken cancellationToken)
        {
            return await _rawMaterial.UpdateAsync(model, cancellationToken);
        }

        public async Task<RawMaterial> DeleteRawMaterial(int Id, CancellationToken cancellationToken)
        {
            var entity = await _rawMaterial.GetByIdAsync(Id, cancellationToken);
            if (entity != null)
            {
                return await _rawMaterial.UpdateAsync(entity, cancellationToken);
            }
            return entity;
        }
    }
}