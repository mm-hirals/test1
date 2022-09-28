using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IPolishDA
    {
        public Task<IQueryable<Polish>> GetAll(CancellationToken cancellationToken);

        public Task<Polish> GetById(int Id, CancellationToken cancellationToken);

        public Task<Polish> CreatePolish(Polish model, CancellationToken cancellationToken);

        public Task<Polish> UpdatePolish(int Id, Polish model, CancellationToken cancellationToken);

        public Task<Polish> DeletePolish(int Id, CancellationToken cancellationToken);
    }
}