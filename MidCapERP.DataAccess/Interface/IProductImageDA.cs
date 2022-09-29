using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IProductImageDA
    {
        public Task<IQueryable<ProductImage>> GetAll(CancellationToken cancellationToken);

        public Task<ProductImage> GetById(Int64 Id, CancellationToken cancellationToken);

        public Task<IQueryable<ProductImage>> GetAllByProductId(Int64 productId, CancellationToken cancellationToken);

        public Task<ProductImage> CreateProductImage(ProductImage model, CancellationToken cancellationToken);

        public Task<ProductImage> UpdateProductImage(ProductImage model, CancellationToken cancellationToken);

        public Task<ProductImage> DeleteProductImage(long Id, CancellationToken cancellationToken);
    }
}