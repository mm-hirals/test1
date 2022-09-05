using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class ProductImageDA : IProductImageDA
    {
        private readonly ISqlRepository<ProductImage> _product;

        public ProductImageDA(ISqlRepository<ProductImage> product)
        {
            _product = product;
        }

        public async Task<ProductImage> CreateProductImage(ProductImage model, CancellationToken cancellationToken)
        {
            return await _product.InsertAsync(model, cancellationToken);
        }
    }
}