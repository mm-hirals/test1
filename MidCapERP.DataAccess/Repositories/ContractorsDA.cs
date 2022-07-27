using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class ContractorsDA : IContractorsDA
    {
        private readonly ISqlRepository<Contractors> _contractors;

        public ContractorsDA(ISqlRepository<Contractors> contractors)
        {
            _contractors = contractors;
        }

        public async Task<IQueryable<Contractors>> GetAllContractor(CancellationToken cancellationToken)
        {
            return await _contractors.GetAsync(cancellationToken);
        }

        public async Task<Contractors> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _contractors.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<Contractors> CreateContractor(Contractors model, CancellationToken cancellationToken)
        {
            return await _contractors.InsertAsync(model, cancellationToken);
        }

        public async Task<Contractors> UpdateContractor(int Id, Contractors model, CancellationToken cancellationToken)
        {
            return await _contractors.UpdateAsync(model, cancellationToken);
        }

        public async Task<Contractors> DeleteContractor(int Id, CancellationToken cancellationToken)
        {
            var entity = await _contractors.GetByIdAsync(Id, cancellationToken);
            if (entity != null)
            {
                return await _contractors.UpdateAsync(entity, cancellationToken);
            }
            return entity;
        }
    }
}