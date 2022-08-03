using MidCapERP.Dto.Accessories;
using MidCapERP.Dto.DataGrid;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IAccessoriesBL
    {
        public Task<IEnumerable<AccessoriesResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<AccessoriesResponseDto>> GetFilterAccessoriesData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<AccessoriesRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<AccessoriesResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken);

        public Task<AccessoriesRequestDto> CreateAccessories(AccessoriesRequestDto model, CancellationToken cancellationToken);

        public Task<AccessoriesRequestDto> UpdateAccessories(int Id, AccessoriesRequestDto model, CancellationToken cancellationToken);

        public Task<AccessoriesRequestDto> DeleteAccessories(int Id, CancellationToken cancellationToken);
    }
}
