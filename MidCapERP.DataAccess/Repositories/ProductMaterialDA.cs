using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class ProductMaterialDA : IProductMaterialDA
    {
        private readonly ISqlRepository<ProductMaterial> _productMaterial;

        public ProductMaterialDA(ISqlRepository<ProductMaterial> productMaterial)
        {
            _productMaterial = productMaterial;
        }

        public async Task<ProductMaterial> GetById(Int64 Id, CancellationToken cancellationToken)
        {
            return await _productMaterial.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<ProductMaterial> CreateProductMaterial(ProductMaterial model, CancellationToken cancellationToken)
        {
            return await _productMaterial.InsertAsync(model, cancellationToken);
        }
    }
}