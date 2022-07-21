using MidCapERP.Dto.Statuses;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IStatusesBL
    {
        public Task<IEnumerable<StatusesResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<StatusesResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken);

        public Task<StatusesRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<StatusesRequestDto> CreateStatuses(StatusesRequestDto model, CancellationToken cancellationToken);

        public Task<StatusesRequestDto> UpdateStatuses(int Id, StatusesRequestDto model, CancellationToken cancellationToken);

        public Task<StatusesRequestDto> DeleteStatuses(int Id, CancellationToken cancellationToken);
    }
}
