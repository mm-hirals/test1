using MidCapERP.Dto.WrkImportCustomers;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IWrkImportCustomersBL
    {
        public Task<IEnumerable<WrkImportCustomersDto>> GetAll(CancellationToken cancellationToken);

        public Task<WrkImportCustomersDto> GetById(long WrkCustomerID, CancellationToken cancellationToken);

        public Task<List<WrkImportCustomersDto>> CreateWrkCustomer(List<WrkImportCustomersDto> model, CancellationToken cancellationToken);
    }
}