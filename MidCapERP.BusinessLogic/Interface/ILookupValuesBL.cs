using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.LookupValues;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface ILookupValuesBL
    {
        public Task<IEnumerable<LookupValuesResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<LookupValuesResponseDto>> GetFilterLookupValuesData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<LookupValuesResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken);

        public Task<LookupValuesRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<LookupValuesRequestDto> CreateLookupValues(LookupValuesRequestDto model, CancellationToken cancellationToken);

        public Task<LookupValuesRequestDto> UpdateLookupValues(int Id, LookupValuesRequestDto model, CancellationToken cancellationToken);

        public Task<LookupValuesRequestDto> DeleteLookupValues(int Id, CancellationToken cancellationToken);
    }
}