using MidCapERP.Dto.ArchitectAddresses;
using MidCapERP.Dto.DataGrid;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IArchitectAddressesBL
    {
        public Task<IEnumerable<ArchitectAddressesResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<ArchitectAddressesResponseDto>> GetFilterArchitectAddressesData(ArchitectAddressDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<ArchitectAddressesRequestDto> CreateArchitectAddresses(ArchitectAddressesRequestDto model, CancellationToken cancellationToken);

        public Task<ArchitectAddressesRequestDto> GetById(Int64 Id, CancellationToken cancellationToken);

        public Task<IEnumerable<ArchitectAddressesResponseDto>> GetArchitectById(Int64 Id, CancellationToken cancellationToken);

        public Task<ArchitectAddressesRequestDto> UpdateArchitectAddresses(Int64 Id, ArchitectAddressesRequestDto model, CancellationToken cancellationToken);

        public Task<ArchitectAddressesRequestDto> DeleteArchitectAddresses(Int64 Id, CancellationToken cancellationToken);
    }
}