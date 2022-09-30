using MidCapERP.Dto.CustomerAddresses;
using MidCapERP.Dto.DataGrid;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IArchitectAddressesBL
    {
        public Task<IEnumerable<CustomerAddressesResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<CustomerAddressesResponseDto>> GetFilterArchitectAddressesData(CustomerAddressDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<CustomerAddressesRequestDto> CreateArchitectAddresses(CustomerAddressesRequestDto model, CancellationToken cancellationToken);

        public Task<CustomerAddressesRequestDto> GetById(Int64 Id, CancellationToken cancellationToken);

        public Task<IEnumerable<CustomerAddressesResponseDto>> GetArchitectById(Int64 Id, CancellationToken cancellationToken);

        public Task<CustomerAddressesRequestDto> UpdateArchitectAddresses(Int64 Id, CustomerAddressesRequestDto model, CancellationToken cancellationToken);

        public Task<CustomerAddressesRequestDto> DeleteArchitectAddresses(Int64 Id, CancellationToken cancellationToken);
    }
}