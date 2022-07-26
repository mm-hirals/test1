using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IContractorCategoryMappingDA
    {
        public Task<IQueryable<ContractorCategoryMapping>> GetAll(CancellationToken cancellationToken);

        public Task<ContractorCategoryMapping> GetById(int Id, CancellationToken cancellationToken);

        public Task<ContractorCategoryMapping> CreateContractorCategoryMapping(ContractorCategoryMapping model, CancellationToken cancellationToken);

        public Task<ContractorCategoryMapping> UpdateContractorCategoryMapping(int Id, ContractorCategoryMapping model, CancellationToken cancellationToken);

        public Task<ContractorCategoryMapping> DeleteContractorCategoryMapping(int Id, CancellationToken cancellationToken);
    }
}