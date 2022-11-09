using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Unit;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IUnitBL
    {
        public Task<IEnumerable<UnitResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<UnitResponseDto>> GetFilterUnitData(UnitDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<UnitResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken);

        public Task<UnitRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<UnitRequestDto> CreateUnit(UnitRequestDto model, CancellationToken cancellationToken);

        public Task<UnitRequestDto> UpdateUnit(int Id, UnitRequestDto model, CancellationToken cancellationToken);

        public Task<UnitRequestDto> DeleteUnit(int Id, CancellationToken cancellationToken);

        public Task<bool> ValidateUnitName(UnitRequestDto unitRequestDto, CancellationToken cancellationToken);
    }
}