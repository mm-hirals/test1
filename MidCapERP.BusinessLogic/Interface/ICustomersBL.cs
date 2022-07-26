using MidCapERP.Dto.Customers;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface ICustomersBL
    {
        public Task<IEnumerable<CustomersResponseDto>> GetAllCustomers(CancellationToken cancellationToken);

        public Task<CustomersResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken);

        public Task<CustomersRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<CustomersRequestDto> CreateCustomers(CustomersRequestDto model, CancellationToken cancellationToken);

        public Task<CustomersRequestDto> UpdateCustomers(int Id, CustomersRequestDto model, CancellationToken cancellationToken);

        public Task<CustomersRequestDto> DeleteCustomers(int Id, CancellationToken cancellationToken);
    }
}
