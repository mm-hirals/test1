using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.WoodType;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IWoodTypeBL
    {
        public Task<IEnumerable<WoodTypeResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<WoodTypeResponseDto>> GetFilterWoodTypeData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<WoodTypeResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken);

        public Task<WoodTypeRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<WoodTypeRequestDto> CreateWoodType(WoodTypeRequestDto model, CancellationToken cancellationToken);

        public Task<WoodTypeRequestDto> UpdateWoodType(int Id, WoodTypeRequestDto model, CancellationToken cancellationToken);

        public Task<WoodTypeRequestDto> DeleteWoodType(int Id, CancellationToken cancellationToken);
    }
}
