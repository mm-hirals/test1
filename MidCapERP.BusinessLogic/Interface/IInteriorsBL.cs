using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Interior;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IInteriorsBL
    {
        public Task<IEnumerable<InteriorResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<InteriorResponseDto>> GetFilterInteriorsData(InteriorDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<InteriorRequestDto> GetById(Int64 Id, CancellationToken cancellationToken);

        public Task<InteriorRequestDto> CreateInteriors(InteriorRequestDto model, CancellationToken cancellationToken);

        public Task<InteriorRequestDto> UpdateInteriors(Int64 Id, InteriorRequestDto model, CancellationToken cancellationToken);

        public Task SendSMSToInteriors(InteriorsSendSMSDto model, CancellationToken cancellationToken);

        public Task<bool> ValidateInteriorPhoneNumber(InteriorRequestDto interiorRequestDto, CancellationToken cancellationToken);
    }
}