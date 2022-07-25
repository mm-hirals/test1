using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class LookupsDA : ILookupsDA
    {
        private readonly ISqlRepository<Lookups> _lookups;

        public LookupsDA(ISqlRepository<Lookups> lookups)
        {
            _lookups = lookups;
        }

        public async Task<IQueryable<Lookups>> GetAll(CancellationToken cancellationToken)
        {
            return await _lookups.GetAsync(cancellationToken);
        }

        public async Task<Lookups> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _lookups.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<Lookups> CreateLookup(Lookups model, CancellationToken cancellationToken)
        {
            return await _lookups.InsertAsync(model, cancellationToken);
        }

        public async Task<Lookups> UpdateLookup(int Id, Lookups model, CancellationToken cancellationToken)
        {
            return await _lookups.UpdateAsync(model, cancellationToken);
        }

        public async Task<Lookups> DeleteLookup(int Id, CancellationToken cancellationToken)
        {
            var entity = await _lookups.GetByIdAsync(Id, cancellationToken);
            if (entity != null)
            {
                return await _lookups.UpdateAsync(entity, cancellationToken);
            }
            return entity;
        }
    }
}
