using MidCapERP.Dto.Architect;
using MidCapERP.Dto.DataGrid;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IArchitectsBL
    {
        public Task<IEnumerable<ArchitectResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<ArchitectResponseDto>> GetFilterArchitectsData(ArchitectDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<ArchitectRequestDto> GetById(Int64 Id, CancellationToken cancellationToken);

        public Task<ArchitectRequestDto> CreateArchitects(ArchitectRequestDto model, CancellationToken cancellationToken);

        public Task<ArchitectRequestDto> UpdateArchitects(Int64 Id, ArchitectRequestDto model, CancellationToken cancellationToken);

        public Task SendSMSToArchitects(ArchitectsSendSMSDto model, CancellationToken cancellationToken);
    }
}