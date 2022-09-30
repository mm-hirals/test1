using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Polish;
using MidCapERP.Dto.SearchResponse;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IPolishBL
    {
        public Task<IEnumerable<PolishResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<PolishResponseDto>> GetFilterPolishData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<PolishResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken);

        public Task<PolishRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<IList<SearchResponse>> GetPolishForDropDownByModuleNo(string modelno, CancellationToken cancellation);

        public Task<PolishApiResponseDto> GetPolishForDetailsByModuleNo(string detailsModelNo, CancellationToken cancellation);

        public Task<PolishRequestDto> CreatePolish(PolishRequestDto model, CancellationToken cancellationToken);

        public Task<PolishRequestDto> UpdatePolish(int Id, PolishRequestDto model, CancellationToken cancellationToken);

        public Task<PolishRequestDto> DeletePolish(int Id, CancellationToken cancellationToken);
    }
}