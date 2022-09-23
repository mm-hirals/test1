using MidCapERP.Dto.Customers;
using MidCapERP.Dto.CustomersTypes;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.MegaSearch;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface ICustomersBL
    {
        public Task<IEnumerable<CustomersResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<CustomersResponseDto> GetCustomerByMobileNumberOrEmailId(string phoneNumberOrEmailId, CancellationToken cancellationToken);

        public Task<IEnumerable<CustomersTypesResponseDto>> CustomersTypesGetAll(CancellationToken cancellationToken);

        public Task<bool> CheckCustomerExistOrNot(string phoneNumberOrEmail, CancellationToken cancellationToken);

        public Task<IEnumerable<CustomersResponseDto>> SearchCustomer(string customerNameOrEmailOrMobileNo, CancellationToken cancellationToken);

        public Task<JsonRepsonse<CustomersResponseDto>> GetFilterCustomersData(CustomerDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<IEnumerable<MegaSearchResponse>> GetCustomerForDropDownByMobileNo(string searchText, CancellationToken cancellation);

        public Task<CustomersResponseDto> GetCustomerForDetailsByMobileNo(string searchText, CancellationToken cancellation);

        public Task<CustomersTypesResponseDto> CustomersTypesGetDetailsById(Int64 Id, CancellationToken cancellationToken);

        public Task<CustomersRequestDto> GetById(Int64 Id, CancellationToken cancellationToken);

        public Task<CustomersRequestDto> CreateCustomers(CustomersRequestDto model, CancellationToken cancellationToken);

        public Task<CustomerApiRequestDto> CreateCustomerApi(CustomerApiRequestDto model, CancellationToken cancellationToken);

        public Task<CustomersRequestDto> UpdateCustomers(Int64 Id, CustomersRequestDto model, CancellationToken cancellationToken);

        public Task<int> GetCustomerCount(CancellationToken cancellationToken);
    }
}