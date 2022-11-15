using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Customers;
using MidCapERP.Dto.CustomersTypes;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.MegaSearch;
using MidCapERP.Dto.WrkImportCustomers;
using MidCapERP.Dto.WrkImportFiles;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface ICustomersBL
    {
        public Task<IEnumerable<CustomersResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<CustomersApiResponseDto> GetCustomerById(long customerId, CancellationToken cancellationToken);

        public Task<IEnumerable<CustomerApiDropDownResponceDto>> GetSearchCustomerForDropDownNameOrPhoneNumber(string searchText, CancellationToken cancellationToken);

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

        public Task<CustomerApiRequestDto> UpdateCustomerApi(Int64 Id, CustomerApiRequestDto model, CancellationToken cancellationToken);

        public Task<CustomersRequestDto> UpdateCustomers(Int64 Id, CustomersRequestDto model, CancellationToken cancellationToken);

        public Task<int> GetCustomerCount(CancellationToken cancellationToken);

        public Task SendSMSToCustomers(CustomersSendSMSDto model, CancellationToken cancellationToken);

        public Task<CustomersApiResponseDto> GetCustomerByIdAPI(Int64 id, CancellationToken cancellationToken);

        public Task<bool> ValidateCustomerPhoneNumber(CustomersRequestDto customerRequestDto, CancellationToken cancellationToken);

        public List<WrkImportCustomersDto> CustomerFileImport(WrkImportFilesRequestDto entity, long WrkImportFileID);

        public Task ImportCustomers(long WrkImportFileID, CancellationToken cancellationToken);
        public Task<OTPLogin> SendCustomerOtpAPI(CustomerApiRequestDto model, CancellationToken cancellationToken);

        public Task<bool> ValidateCustomerOtpAPI(CustomersRequestOtpDto model, CancellationToken cancellationToken);

        public Task<OTPLogin> ResendCustomerOtpAPI(CustomersRequestOtpDto model, CancellationToken cancellationToken);
    }
}