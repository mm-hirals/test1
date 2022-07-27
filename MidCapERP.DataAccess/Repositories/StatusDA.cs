using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class StatusDA : IStatusDA
    {
        private readonly ISqlRepository<Statuses> _status;

        public StatusDA(ISqlRepository<Statuses> status)
        {
            _status = status;
        }

        public async Task<IQueryable<Statuses>> GetAll(CancellationToken cancellationToken)
        {
            return await _status.GetAsync(cancellationToken);
        }

        public async Task<Statuses> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _status.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<Statuses> CreateStatus(Statuses model, CancellationToken cancellationToken)
        {
            return await _status.InsertAsync(model, cancellationToken);
        }

        public async Task<Statuses> UpdateStatus(int Id, Statuses model, CancellationToken cancellationToken)
        {
            return await _status.UpdateAsync(model, cancellationToken);
        }

        public async Task<Statuses> DeleteStatus(int Id, CancellationToken cancellationToken)
        {
            var entity = await _status.GetByIdAsync(Id, cancellationToken);
            if (entity != null)
            {
                return await _status.UpdateAsync(entity, cancellationToken);
            }
            return entity;
        }
    }
}