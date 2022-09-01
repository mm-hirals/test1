using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Frame;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IFrameBL
    {
        public Task<IEnumerable<FrameResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<FrameResponseDto>> GetFilterFrameData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<FrameResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken);

        public Task<FrameRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<FrameRequestDto> CreateFrame(FrameRequestDto model, CancellationToken cancellationToken);

        public Task<FrameRequestDto> UpdateFrame(int Id, FrameRequestDto model, CancellationToken cancellationToken);

        public Task<FrameRequestDto> DeleteFrame(int Id, CancellationToken cancellationToken);
    }
}