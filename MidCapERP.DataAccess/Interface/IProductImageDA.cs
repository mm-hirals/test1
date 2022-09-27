using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IProductImageDA
    {
        public Task<IQueryable<ProductImage>> GetAll(CancellationToken cancellationToken);

        public Task<ProductImage> CreateProductImage(ProductImage model, CancellationToken cancellationToken);

        public Task<ProductImage> DeleteProductImage(long Id, CancellationToken cancellationToken);
    }
}