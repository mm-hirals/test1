using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.InteriorAddresses;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IInteriorAddressesBL
    {
        public Task<IEnumerable<InteriorAddressesResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<InteriorAddressesResponseDto>> GetFilterInteriorAddressesData(InteriorAddressDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<InteriorAddressesRequestDto> CreateInteriorAddresses(InteriorAddressesRequestDto model, CancellationToken cancellationToken);

        public Task<InteriorAddressesRequestDto> GetById(Int64 Id, CancellationToken cancellationToken);

        public Task<IEnumerable<InteriorAddressesResponseDto>> GetInteriorById(Int64 Id, CancellationToken cancellationToken);

        public Task<InteriorAddressesRequestDto> UpdateInteriorAddresses(Int64 Id, InteriorAddressesRequestDto model, CancellationToken cancellationToken);

        public Task<InteriorAddressesRequestDto> DeleteInteriorAddresses(Int64 Id, CancellationToken cancellationToken);
    }
}