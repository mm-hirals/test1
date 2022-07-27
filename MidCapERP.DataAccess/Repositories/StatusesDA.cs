using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class StatusesDA : IStatusesDA
    {
        private readonly ISqlRepository<Statuses> _statuses;

        public StatusesDA(ISqlRepository<Statuses> statuses)
        {
            _statuses = statuses;
        }

        public async Task<IQueryable<Statuses>> GetAll(CancellationToken cancellationToken)
        {
            return await _statuses.GetAsync(cancellationToken);
        }

        public async Task<Statuses> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _statuses.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<Statuses> CreateStatuses(Statuses model, CancellationToken cancellationToken)
        {
            return await _statuses.InsertAsync(model, cancellationToken);
        }

        public async Task<Statuses> UpdateStatuses(int Id, Statuses model, CancellationToken cancellationToken)
        {
            return await _statuses.UpdateAsync(model, cancellationToken);
        }

        public async Task<Statuses> DeleteStatuses(int Id, CancellationToken cancellationToken)
        {
            var entity = await _statuses.GetByIdAsync(Id, cancellationToken);
            if (entity != null)
            {
                return await _statuses.UpdateAsync(entity, cancellationToken);
            }
            return entity;
        }
    }
}