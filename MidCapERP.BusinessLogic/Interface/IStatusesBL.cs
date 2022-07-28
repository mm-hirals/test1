using MidCapERP.Dto.Status;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IStatusBL
    {
        public Task<IEnumerable<StatusResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<StatusRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<StatusRequestDto> CreateStatus(StatusRequestDto model, CancellationToken cancellationToken);

        public Task<StatusRequestDto> UpdateStatus(int Id, StatusRequestDto model, CancellationToken cancellationToken);

        public Task<StatusRequestDto> DeleteStatus(int Id, CancellationToken cancellationToken);
    }
}