using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface ILookupsDA
    {
        public Task<IQueryable<Lookups>> GetAll(CancellationToken cancellationToken);

        public Task<Lookups> GetById(int Id, CancellationToken cancellationToken);

        public Task<Lookups> CreateLookup(Lookups model, CancellationToken cancellationToken);

        public Task<Lookups> UpdateLookup(int Id, Lookups model, CancellationToken cancellationToken);

        public Task<Lookups> DeleteLookup(int Id, CancellationToken cancellationToken);
    }
}
