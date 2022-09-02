using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.FrameType;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IFrameTypeBL
    {
        public Task<IEnumerable<FrameTypeResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<FrameTypeResponseDto>> GetFilterFrameTypeData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<FrameTypeResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken);

        public Task<FrameTypeRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<FrameTypeRequestDto> CreateFrameType(FrameTypeRequestDto model, CancellationToken cancellationToken);

        public Task<FrameTypeRequestDto> UpdateFrameType(int Id, FrameTypeRequestDto model, CancellationToken cancellationToken);

        public Task<FrameTypeRequestDto> DeleteFrameType(int Id, CancellationToken cancellationToken);
    }
}