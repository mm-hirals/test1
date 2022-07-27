using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface ILookupValuesDA
    {
        public Task<IQueryable<LookupValues>> GetAll(CancellationToken cancellationToken);

        public Task<LookupValues> GetById(int Id, CancellationToken cancellationToken);

        public Task<LookupValues> CreateLookupValue(LookupValues model, CancellationToken cancellationToken);

        public Task<LookupValues> UpdateLookupValue(int Id, LookupValues model, CancellationToken cancellationToken);

        public Task<LookupValues> DeleteLookupValue(int Id, CancellationToken cancellationToken);
    }
}