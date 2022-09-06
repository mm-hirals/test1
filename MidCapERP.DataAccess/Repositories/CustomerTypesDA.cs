using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class CustomerTypesDA : ICustomerTypesDA
    {
        private readonly ISqlRepository<CustomerTypes> _customersTypes;

        public CustomerTypesDA(ISqlRepository<CustomerTypes> customersTypes)
        {
            _customersTypes = customersTypes;
        }

        public async Task<IQueryable<CustomerTypes>> GetAll(CancellationToken cancellationToken)
        {
            return await _customersTypes.GetAsync(cancellationToken, x => x.IsDeleted == false);
        }

        public async Task<CustomerTypes> GetById(Int64 Id, CancellationToken cancellationToken)
        {
            return await _customersTypes.GetByIdAsync(Id, cancellationToken);
        }
    }
}