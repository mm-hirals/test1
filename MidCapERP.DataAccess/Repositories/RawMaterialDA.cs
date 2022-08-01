using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class RawMaterialDA : IRawMaterialDA
    {
        private readonly ISqlRepository<RawMaterial> _RawMaterial;

        public RawMaterialDA(ISqlRepository<RawMaterial> rawMaterial)
        {
            _RawMaterial = rawMaterial;
        }

        public async Task<IQueryable<RawMaterial>> GetAll(CancellationToken cancellationToken)
        {
            return await _RawMaterial.GetAsync(cancellationToken);
        }

        public async Task<RawMaterial> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _RawMaterial.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<RawMaterial> CreateRawMaterial(RawMaterial model, CancellationToken cancellationToken)
        {
            return await _RawMaterial.InsertAsync(model, cancellationToken);
        }

        public async Task<RawMaterial> UpdateRawMaterial(int Id, RawMaterial model, CancellationToken cancellationToken)
        {
            return await _RawMaterial.UpdateAsync(model, cancellationToken);
        }

        public async Task<RawMaterial> DeleteRawMaterial(int Id, CancellationToken cancellationToken)
        {
            var entity = await _RawMaterial.GetByIdAsync(Id, cancellationToken);
            if (entity != null)
            {
                return await _RawMaterial.UpdateAsync(entity, cancellationToken);
            }
            return entity;
        }
    }
}
