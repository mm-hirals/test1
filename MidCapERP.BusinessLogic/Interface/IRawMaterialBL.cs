using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.RawMaterial;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IRawMaterialBL
    {
        public Task<IEnumerable<RawMaterialResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<RawMaterialResponseDto>> GetFilterRawMaterialData(RawMaterialDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<RawMaterialResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken);

        public Task<RawMaterialRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<RawMaterialRequestDto> CreateRawMaterial(RawMaterialRequestDto model, CancellationToken cancellationToken);

        public Task<RawMaterialRequestDto> UpdateRawMaterial(int Id, RawMaterialRequestDto model, CancellationToken cancellationToken);

        public Task<RawMaterialRequestDto> DeleteRawMaterial(int Id, CancellationToken cancellationToken);

        public Task<bool> ValidateRawMaterialTitle(RawMaterialRequestDto rawMaterialRequestDto, CancellationToken cancellationToken);
    }
}