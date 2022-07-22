using MidCapERP.Dto.ContractorCategoryMapping;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IContractorCategoryMappingBL
    {
        public Task<IEnumerable<ContractorCategoryMappingResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<ContractorCategoryMappingRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<ContractorCategoryMappingRequestDto> CreateContractorCategoryMapping(ContractorCategoryMappingRequestDto model, CancellationToken cancellationToken);

        public Task<ContractorCategoryMappingRequestDto> UpdateContractorCategoryMapping(int Id, ContractorCategoryMappingRequestDto model, CancellationToken cancellationToken);

        public Task<ContractorCategoryMappingRequestDto> DeleteContractorCategoryMapping(int Id, CancellationToken cancellationToken);
    }
}