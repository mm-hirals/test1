using MidCapERP.Dto.Contractors;
using MidCapERP.Dto.DataGrid;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IContractorsBL
    {
        public Task<IEnumerable<ContractorsResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<ContractorsResponseDto>> GetFilterContractorData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<ContractorsResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken);

        public Task<ContractorsRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<ContractorsRequestDto> CreateContractor(ContractorsRequestDto model, CancellationToken cancellationToken);

        public Task<ContractorsRequestDto> UpdateContractor(int Id, ContractorsRequestDto model, CancellationToken cancellationToken);

        public Task<ContractorsRequestDto> DeleteContractor(int Id, CancellationToken cancellationToken);
    }
}