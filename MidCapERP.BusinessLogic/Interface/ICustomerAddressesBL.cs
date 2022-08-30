using MidCapERP.Dto.CustomerAddresses;
using MidCapERP.Dto.DataGrid;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface ICustomerAddressesBL
    {
        public Task<IEnumerable<CustomerAddressesResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<CustomerAddressesResponseDto>> GetFilterCustomerAddressesData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);
    }
}