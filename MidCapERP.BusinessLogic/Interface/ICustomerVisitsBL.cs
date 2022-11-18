using MidCapERP.Dto.CustomerVisit;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface ICustomerVisitsBL
    {
        public Task<CustomerVisitRequestDto> CustomerVisitAPI(CustomerVisitRequestDto model, CancellationToken cancellation);

        public Task<IEnumerable<CustomerVisitResponseDto>> GetAll(CancellationToken cancellationToken);
        public Task<IEnumerable<CustomerVisitResponseDto>> GetById(long customerId, CancellationToken cancellationToken);
    }
}