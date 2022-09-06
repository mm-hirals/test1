using MidCapERP.Dto.CustomerAddresses;
using MidCapERP.Dto.DataGrid;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface ICustomerAddressesBL
    {
        public Task<IEnumerable<CustomerAddressesResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<CustomerAddressesResponseDto>> GetFilterCustomerAddressesData(CustomerAddressDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<CustomerAddressesRequestDto> CreateCustomerAddresses(CustomerAddressesRequestDto model, CancellationToken cancellationToken);

        public Task<CustomerAddressesRequestDto> GetById(Int64 Id, CancellationToken cancellationToken);

        public Task<CustomerAddressesRequestDto> UpdateCustomerAddresses(Int64 Id, CustomerAddressesRequestDto model, CancellationToken cancellationToken);
    }
}