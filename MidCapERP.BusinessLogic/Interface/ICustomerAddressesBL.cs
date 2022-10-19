using MidCapERP.Dto.CustomerAddresses;
using MidCapERP.Dto.DataGrid;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface ICustomerAddressesBL
    {
        public Task<IEnumerable<CustomerAddressesResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<CustomerAddressesResponseDto>> GetFilterCustomerAddressesData(CustomerAddressDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<CustomerAddressesRequestDto> CreateCustomerAddresses(CustomerAddressesRequestDto model, CancellationToken cancellationToken);

        public Task<CustomerAddressesApiRequestDto> CreateCustomerApiAddresses(CustomerAddressesApiRequestDto model, CancellationToken cancellationToken);

        public Task<CustomerAddressesRequestDto> GetById(Int64 Id, CancellationToken cancellationToken);

        public Task<IEnumerable<CustomerAddressesResponseDto>> GetCustomerById(Int64 Id, CancellationToken cancellationToken);

        public Task<CustomerAddressesResponseDto> GetCustomerAddressById(Int64 Id, CancellationToken cancellationToken);

        public Task<CustomerAddressesRequestDto> UpdateCustomerAddresses(Int64 Id, CustomerAddressesRequestDto model, CancellationToken cancellationToken);

        public Task<CustomerAddressesApiRequestDto> UpdateCustomerApiAddresses(Int64 Id, CustomerAddressesApiRequestDto model, CancellationToken cancellationToken);

        public Task<CustomerAddressesRequestDto> DeleteCustomerAddresses(Int64 Id, CancellationToken cancellationToken);
    }
}