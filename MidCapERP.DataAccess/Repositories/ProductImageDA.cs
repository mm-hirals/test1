using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using static MidCapERP.Core.Constants.ApplicationIdentityConstants.Permissions;

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

        public async Task<IQueryable<ProductImage>> GetAllByProductId(Int64 productId, CancellationToken cancellationToken)
        {
            return await _productImage.GetAsync(cancellationToken, x=>x.ProductId == productId);
        }

        public async Task<ProductImage> GetById(Int64 Id, CancellationToken cancellationToken)
        {
            return await _productImage.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<ProductImage> CreateProductImage(ProductImage model, CancellationToken cancellationToken)
        {
            return await _productImage.InsertAsync(model, cancellationToken);
        }

        public async Task<ProductImage> UpdateProductImage(ProductImage model, CancellationToken cancellationToken)
        {
            return await _productImage.UpdateAsync(model, cancellationToken);
        }

        public async Task<ProductImage> DeleteProductImage(long Id, CancellationToken cancellationToken)
        {
            var entity = await _productImage.GetByIdAsync(Id, cancellationToken);
            if (entity != null)
            {
                return await _productImage.DeleteAsync(entity, cancellationToken);
            }
            return entity;
        }
    }
}