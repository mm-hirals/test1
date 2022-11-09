namespace MidCapERP.BusinessLogic.Interface
{
    public interface IDashboardBL
    {
        public Task<int> GetOrderCount(CancellationToken cancellationToken);

        public Task<int> GetProductCount(CancellationToken cancellationToken);

        public Task<int> GetCustomerCount(CancellationToken cancellationToken);

        public Task<int> GetInteriorCount(CancellationToken cancellationToken);
    }
}