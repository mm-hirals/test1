using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class PolishDA : IPolishDA
    {
        private readonly ISqlRepository<Polish> _polish;

        public PolishDA(ISqlRepository<Polish> polish)
        {
            _polish = polish;
        }

        public async Task<IQueryable<Polish>> GetAll(CancellationToken cancellationToken)
        {
            return await _polish.GetAsync(cancellationToken, x => x.IsDeleted == false);
        }

        public async Task<Polish> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _polish.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<Polish> CreatePolish(Polish model, CancellationToken cancellationToken)
        {
            return await _polish.InsertAsync(model, cancellationToken);
        }

        public async Task<Polish> UpdatePolish(int Id, Polish model, CancellationToken cancellationToken)
        {
            return await _polish.UpdateAsync(model, cancellationToken);
        }

        public async Task<Polish> DeletePolish(int Id, CancellationToken cancellationToken)
        {
            var entity = await _polish.GetByIdAsync(Id, cancellationToken);
            if (entity != null)
            {
                return await _polish.UpdateAsync(entity, cancellationToken);
            }
            return entity;
        }
    }
}