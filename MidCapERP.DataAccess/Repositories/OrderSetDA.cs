using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;

namespace MidCapERP.DataAccess.Repositories
{
    public class OrderSetDA : IOrderSetDA
    {
        private readonly ISqlRepository<OrderSet> _orderset;

        public OrderSetDA(ISqlRepository<OrderSet> orderset)
        {
            _orderset = orderset;
        }

        public async Task<OrderSet> GetById(long Id, CancellationToken cancellationToken)
        {
            return await _orderset.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<IQueryable<OrderSet>> GetAll(CancellationToken cancellationToken)
        {
            return await _orderset.GetAsync(cancellationToken);
        }

        public async Task<OrderSet> CreateOrderSet(OrderSet model, CancellationToken cancellationToken)
        {
            return await _orderset.InsertAsync(model, cancellationToken);
        }

        public async Task<OrderSet> UpdateOrderSet(OrderSet model, CancellationToken cancellationToken)
        {
            return await _orderset.UpdateAsync(model, cancellationToken);
        }

        public async Task<OrderSet> DeleteOrderSet(OrderSet model, CancellationToken cancellationToken)
        {
            return await _orderset.DeleteAsync(model, cancellationToken);
        }
    }
}