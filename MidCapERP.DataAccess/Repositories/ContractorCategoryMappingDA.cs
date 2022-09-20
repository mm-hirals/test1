using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class ContractorCategoryMappingDA : IContractorCategoryMappingDA
    {
        private readonly ISqlRepository<ContractorCategoryMapping> _contractorCategoryMapping;

        public ContractorCategoryMappingDA(ISqlRepository<ContractorCategoryMapping> contractorCategoryMapping)
        {
            _contractorCategoryMapping = contractorCategoryMapping;
        }

        public async Task<IQueryable<ContractorCategoryMapping>> GetAll(CancellationToken cancellationToken)
        {
            return await _contractorCategoryMapping.GetAsync(cancellationToken, x => x.IsDeleted == false);
        }

        public async Task<ContractorCategoryMapping> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _contractorCategoryMapping.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<ContractorCategoryMapping> CreateContractorCategoryMapping(ContractorCategoryMapping model, CancellationToken cancellationToken)
        {
            return await _contractorCategoryMapping.InsertAsync(model, cancellationToken);
        }

        public async Task<ContractorCategoryMapping> UpdateContractorCategoryMapping(int Id, ContractorCategoryMapping model, CancellationToken cancellationToken)
        {
            return await _contractorCategoryMapping.UpdateAsync(model, cancellationToken);
        }

        public async Task<ContractorCategoryMapping> DeleteContractorCategoryMapping(int Id, CancellationToken cancellationToken)
        {
            var entity = await _contractorCategoryMapping.GetByIdAsync(Id, cancellationToken);
            if (entity != null)
            {
                return await _contractorCategoryMapping.DeleteAsync(entity, cancellationToken);
            }
            return entity;
        }
    }
}