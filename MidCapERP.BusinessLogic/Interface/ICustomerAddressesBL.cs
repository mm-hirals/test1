using MidCapERP.Dto.CustomerAddresses;
using MidCapERP.Dto.DataGrid;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface ICustomerAddressesBL
    {
        public Task<IEnumerable<CustomerAddressesResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<CustomerAddressesResponseDto>> GetFilterCustomerAddressesData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<CustomerAddressesRequestDto> CreateCustomerAddresses(CustomerAddressesRequestDto model, CancellationToken cancellationToken);

        public Task<CustomerAddressesRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<CustomerAddressesRequestDto> UpdateCustomerAddresses(int Id, CustomerAddressesRequestDto model, CancellationToken cancellationToken);

        public Task<CustomerAddressesRequestDto> DeleteCustomerAddresses(int Id, CancellationToken cancellationToken);
    }
}