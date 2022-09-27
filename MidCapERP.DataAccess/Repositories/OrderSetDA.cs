using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;

namespace MidCapERP.DataAccess.Repositories
{
    public class OrderSetDA : IOrderSetDA
    {
        private readonly ISqlRepository<OrderSet> _orderset;
        private readonly CurrentUser _currentUser;

        public OrderSetDA(ISqlRepository<OrderSet> orderset, CurrentUser currentUser)
        {
            _orderset = orderset;
            _currentUser = currentUser;
        }

        public async Task<IQueryable<OrderSet>> GetAll(CancellationToken cancellationToken)
        {
            return await _orderset.GetAsync(cancellationToken);
        }

        public async Task<OrderSet> CreateOrderSet(OrderSet model, CancellationToken cancellationToken)
        {
            return await _orderset.InsertAsync(model, cancellationToken);
        }
    }
}