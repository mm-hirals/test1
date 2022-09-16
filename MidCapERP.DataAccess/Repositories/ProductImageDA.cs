using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class ProductImageDA : IProductImageDA
    {
        private readonly ISqlRepository<ProductImage> _productImage;

        public ProductImageDA(ISqlRepository<ProductImage> productImage)
        {
            _productImage = productImage;
        }

        public async Task<IQueryable<ProductImage>> GetAll(CancellationToken cancellationToken)
        {
            return await _productImage.GetAsync(cancellationToken);
        }

        public async Task<ProductImage> CreateProductImage(ProductImage model, CancellationToken cancellationToken)
        {
            return await _productImage.InsertAsync(model, cancellationToken);
        }
    }
}