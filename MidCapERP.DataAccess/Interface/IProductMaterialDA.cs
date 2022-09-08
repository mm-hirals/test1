using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IProductMaterialDA
    {
        public Task<ProductMaterial> GetById(Int64 Id, CancellationToken cancellationToken);

        public Task<ProductMaterial> CreateProductMaterial(ProductMaterial model, CancellationToken cancellationToken);
    }
}