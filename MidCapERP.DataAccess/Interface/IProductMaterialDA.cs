using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IProductMaterialDA
    {
        public Task<ProductMaterial> CreateProductMaterial(ProductMaterial model, CancellationToken cancellationToken);
    }
}