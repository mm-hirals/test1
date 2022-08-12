using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class ProductDA : IProductDA
    {
        public Task<IQueryable<Products>> GetAll(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Products> CreateProduct(Woods model, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}