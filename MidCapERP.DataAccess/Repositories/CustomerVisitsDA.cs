using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class CustomerVisitsDA : ICustomerVisitsDA
    {
        private ISqlRepository<CustomerVisits> _customerVisits;

        public CustomerVisitsDA(ISqlRepository<CustomerVisits> customerVisits)
        {
            _customerVisits = customerVisits;
        }

        public async Task<CustomerVisits> CreateCustomerVisits(CustomerVisits model, CancellationToken cancellationToken)
        {
            return await _customerVisits.InsertAsync(model, cancellationToken);
        }

        public async Task<IQueryable<CustomerVisits>> GetAll(CancellationToken cancellationToken)
        {
            return await _customerVisits.GetAsync(cancellationToken);
        }
    }
}