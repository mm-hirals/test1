using MidCapERP.Dto.AccessoriesType;
using MidCapERP.Dto.DataGrid;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IAccessoriesTypeBL
    {
        public Task<IEnumerable<AccessoriesTypeResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<AccessoriesTypeResponseDto>> GetFilterAccessoriesTypeData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<AccessoriesTypeRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<AccessoriesTypeResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken);

        public Task<AccessoriesTypeRequestDto> CreateAccessoriesType(AccessoriesTypeRequestDto model, CancellationToken cancellationToken);

        public Task<AccessoriesTypeRequestDto> UpdateAccessoriesType(int Id, AccessoriesTypeRequestDto model, CancellationToken cancellationToken);

        public Task<AccessoriesTypeRequestDto> DeleteAccessoriesType(int Id, CancellationToken cancellationToken);
    }
}