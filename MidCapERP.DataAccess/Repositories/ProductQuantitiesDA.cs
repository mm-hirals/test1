using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class ProductQuantitiesDA : IProductQuantitiesDA
    {
        private readonly ISqlRepository<ProductQuantities> _productQuantities;

        public ProductQuantitiesDA(ISqlRepository<ProductQuantities> productQuantities)
        {
            _productQuantities = productQuantities;
        }

        public async Task<IQueryable<ProductQuantities>> GetAll(CancellationToken cancellationToken)
        {
            return await _productQuantities.GetAsync(cancellationToken);
        }

        public async Task<ProductQuantities> GetById(long Id, CancellationToken cancellationToken)
        {
            return await _productQuantities.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<ProductQuantities> UpdateProductQuantities(long Id, ProductQuantities model, CancellationToken cancellationToken)
        {
            return await _productQuantities.UpdateAsync(model, cancellationToken);
        }
    }
}