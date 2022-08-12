using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IProductDA
    {
        public Task<IQueryable<Products>> GetAll(CancellationToken cancellationToken);

        public Task<Products> CreateProduct(Woods model, CancellationToken cancellationToken);
    }
}