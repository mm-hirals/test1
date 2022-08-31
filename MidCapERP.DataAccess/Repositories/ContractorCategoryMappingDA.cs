using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class ContractorCategoryMappingDA : IContractorCategoryMappingDA
    {
        private readonly ISqlRepository<ContractorCategoryMapping> _ContractorCategoryMapping;

        public ContractorCategoryMappingDA(ISqlRepository<ContractorCategoryMapping> contractorCategoryMapping)
        {
            _ContractorCategoryMapping = contractorCategoryMapping;
        }

        public async Task<IQueryable<ContractorCategoryMapping>> GetAll(CancellationToken cancellationToken)
        {
            return await _ContractorCategoryMapping.GetAsync(cancellationToken);
        }

        public async Task<ContractorCategoryMapping> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _ContractorCategoryMapping.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<ContractorCategoryMapping> CreateContractorCategoryMapping(ContractorCategoryMapping model, CancellationToken cancellationToken)
        {
            return await _ContractorCategoryMapping.InsertAsync(model, cancellationToken);
        }

        public async Task<ContractorCategoryMapping> UpdateContractorCategoryMapping(int Id, ContractorCategoryMapping model, CancellationToken cancellationToken)
        {
            return await _ContractorCategoryMapping.UpdateAsync(model, cancellationToken);
        }

        public async Task<ContractorCategoryMapping> DeleteContractorCategoryMapping(int Id, CancellationToken cancellationToken)
        {
            var entity = await _ContractorCategoryMapping.GetByIdAsync(Id, cancellationToken);
            if (entity != null)
            {
                return await _ContractorCategoryMapping.DeleteAsync(entity, cancellationToken);
            }
            return entity;
        }
    }
}