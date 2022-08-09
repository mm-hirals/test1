using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IContractorsDA
    {
        public Task<IQueryable<Contractors>> GetAll(CancellationToken cancellationToken);

        public Task<Contractors> GetById(int Id, CancellationToken cancellationToken);

        public Task<Contractors> CreateContractor(Contractors model, CancellationToken cancellationToken);

        public Task<Contractors> UpdateContractor(int Id, Contractors model, CancellationToken cancellationToken);

        public Task<Contractors> DeleteContractor(int Id, CancellationToken cancellationToken);
    }
}