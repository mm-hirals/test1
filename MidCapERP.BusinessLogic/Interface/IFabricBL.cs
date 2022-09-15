using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Fabric;
using MidCapERP.Dto.Product;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IFabricBL
    {
        public Task<IEnumerable<FabricResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<FabricResponseDto>> GetFilterFabricData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<FabricResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken);

        public Task<FabricRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<IEnumerable<ProductForDorpDownByModuleNoResponseDto>> GetFabricForDropDownByModuleNo(string modelno, CancellationToken cancellation);

        public Task<FabricResponseDto> GetFabricForDetailsByModuleNo(string modelno, CancellationToken cancellation);

        public Task<FabricRequestDto> CreateFabric(FabricRequestDto model, CancellationToken cancellationToken);

        public Task<FabricRequestDto> UpdateFabric(int Id, FabricRequestDto model, CancellationToken cancellationToken);

        public Task<FabricRequestDto> DeleteFabric(int Id, CancellationToken cancellationToken);
    }
}