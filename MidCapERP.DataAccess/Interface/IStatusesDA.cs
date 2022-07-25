using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IStatusesDA
    {
        public Task<IQueryable<Statuses>> GetAll(CancellationToken cancellationToken);

        public Task<Statuses> GetById(int Id, CancellationToken cancellationToken);

        public Task<Statuses> CreateStatuses(Statuses model, CancellationToken cancellationToken);

        public Task<Statuses> UpdateStatuses(int Id, Statuses model, CancellationToken cancellationToken);

        public Task<Statuses> DeleteStatuses(int Id, CancellationToken cancellationToken);
    }
}
