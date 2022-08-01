using MidCapERP.Dto.AccessoriesTypes;
using MidCapERP.Dto.DataGrid;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IAccessoriesTypesBL
    {
        public Task<IEnumerable<AccessoriesTypesResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<AccessoriesTypesResponseDto>> GetFilterAccessoriesTypesData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<AccessoriesTypesRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<AccessoriesTypesResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken);

        public Task<AccessoriesTypesRequestDto> CreateAccessoriesTypes(AccessoriesTypesRequestDto model, CancellationToken cancellationToken);

        public Task<AccessoriesTypesRequestDto> UpdateAccessoriesTypes(int Id, AccessoriesTypesRequestDto model, CancellationToken cancellationToken);

        public Task<AccessoriesTypesRequestDto> DeleteAccessoriesTypes(int Id, CancellationToken cancellationToken);
    }
}