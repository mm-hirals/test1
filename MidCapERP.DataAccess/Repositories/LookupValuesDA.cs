using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class LookupValuesDA : ILookupValuesDA
    {
        private readonly ISqlRepository<LookupValues> _lookupValues;

        public LookupValuesDA(ISqlRepository<LookupValues> lookupValues)
        {
            _lookupValues = lookupValues;
        }

        public async Task<IQueryable<LookupValues>> GetAll(CancellationToken cancellationToken)
        {
            return await _lookupValues.GetAsync(cancellationToken);
        }

        public async Task<LookupValues> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _lookupValues.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<LookupValues> CreateLookupValue(LookupValues model, CancellationToken cancellationToken)
        {
            return await _lookupValues.InsertAsync(model, cancellationToken);
        }

        public async Task<LookupValues> UpdateLookupValue(int Id, LookupValues model, CancellationToken cancellationToken)
        {
            return await _lookupValues.UpdateAsync(model, cancellationToken);
        }

        public async Task<LookupValues> DeleteLookupValue(int Id, CancellationToken cancellationToken)
        {
            var entity = await _lookupValues.GetByIdAsync(Id, cancellationToken);
            if (entity != null)
            {
                return await _lookupValues.UpdateAsync(entity, cancellationToken);
            }
            return entity;
        }
    }
}