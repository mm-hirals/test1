using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class ProductDA : IProductDA
    {
        private readonly ISqlRepository<Products> _Product;

        public ProductDA(ISqlRepository<Products> product)
        {
            _Product = product;
        }

        public async Task<IQueryable<Products>> GetAll(CancellationToken cancellationToken)
        {
            return await _Product.GetAsync(cancellationToken, x => x.IsDeleted == false);
        }

        public async Task<Products> CreateProduct(Products model, CancellationToken cancellationToken)
        {
            return await _Product.InsertAsync(model, cancellationToken);
        }
    }
}