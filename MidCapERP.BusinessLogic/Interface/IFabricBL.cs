using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Fabric;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IFabricBL
    {
        public Task<JsonRepsonse<FabricResponseDto>> GetFilterFabricData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<FabricRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<FabricRequestDto> CreateFabric(FabricRequestDto model, CancellationToken cancellationToken);

        public Task<FabricRequestDto> UpdateFabric(int Id, FabricRequestDto model, CancellationToken cancellationToken);

        public Task<FabricRequestDto> DeleteFabric(int Id, CancellationToken cancellationToken);
    }
}