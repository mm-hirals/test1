using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Wood;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IWoodBL
    {
        public Task<IEnumerable<WoodResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<WoodResponseDto>> GetFilterWoodData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<WoodResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken);

        public Task<WoodRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<WoodRequestDto> CreateWood(WoodRequestDto model, CancellationToken cancellationToken);

        public Task<WoodRequestDto> UpdateWood(int Id, WoodRequestDto model, CancellationToken cancellationToken);

        public Task<WoodRequestDto> DeleteWood(int Id, CancellationToken cancellationToken);
    }
}