using MidCapERP.Dto.Customers;
using MidCapERP.Dto.DataGrid;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IArchitectsBL
    {
        public Task<IEnumerable<CustomersResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<CustomersResponseDto>> GetFilterArchitectsData(CustomerDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<CustomersRequestDto> GetById(Int64 Id, CancellationToken cancellationToken);

        public Task<CustomersRequestDto> CreateArchitects(CustomersRequestDto model, CancellationToken cancellationToken);

        public Task<CustomersRequestDto> UpdateArchitects(Int64 Id, CustomersRequestDto model, CancellationToken cancellationToken);
    }
}