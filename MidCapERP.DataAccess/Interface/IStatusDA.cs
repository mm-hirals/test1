using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IStatusDA
    {
        public Task<IQueryable<Statuses>> GetAll(CancellationToken cancellationToken);

        public Task<Statuses> GetById(int Id, CancellationToken cancellationToken);

        public Task<Statuses> CreateStatus(Statuses model, CancellationToken cancellationToken);

        public Task<Statuses> UpdateStatus(int Id, Statuses model, CancellationToken cancellationToken);

        public Task<Statuses> DeleteStatus(int Id, CancellationToken cancellationToken);
    }
}