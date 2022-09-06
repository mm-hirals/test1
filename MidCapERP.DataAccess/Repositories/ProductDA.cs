using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class ProductDA : IProductDA
    {
        private readonly ISqlRepository<Product> _Product;

        public ProductDA(ISqlRepository<Product> product)
        {
            _Product = product;
        }

        public async Task<IQueryable<Product>> GetAll(CancellationToken cancellationToken)
        {
            return await _Product.GetAsync(cancellationToken, x => x.IsDeleted == false);
        }

        public async Task<Product> GetById(Int64 Id, CancellationToken cancellationToken)
        {
            return await _Product.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<Product> CreateProduct(Product model, CancellationToken cancellationToken)
        {
            return await _Product.InsertAsync(model, cancellationToken);
        }
    }
}