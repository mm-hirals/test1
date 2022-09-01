using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IProductDA
    {
        public Task<IQueryable<Product>> GetAll(CancellationToken cancellationToken);

        public Task<Product> CreateProduct(Product model, CancellationToken cancellationToken);
    }
}