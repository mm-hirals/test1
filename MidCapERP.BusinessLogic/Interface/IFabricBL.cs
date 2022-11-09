using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Fabric;
using MidCapERP.Dto.Product;
using MidCapERP.Dto.SearchResponse;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IFabricBL
    {
        public Task<IEnumerable<FabricResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<FabricResponseDto>> GetFilterFabricData(FabricDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<FabricResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken);

        public Task<FabricRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<IEnumerable<SearchResponse>> GetFabricForDropDownByModuleNo(string modelno, CancellationToken cancellation);

        public Task<ProductForDetailsByModuleNoResponceDto> GetFabricForDetailsByModuleNo(string modelno, CancellationToken cancellation);

        public Task<FabricRequestDto> CreateFabric(FabricRequestDto model, CancellationToken cancellationToken);

        public Task<FabricRequestDto> UpdateFabric(int Id, FabricRequestDto model, CancellationToken cancellationToken);

        public Task<FabricRequestDto> DeleteFabric(int Id, CancellationToken cancellationToken);

        public Task<bool> ValidateModelNo(FabricRequestDto fabricRequestDto, CancellationToken cancellationToken);
    }
}