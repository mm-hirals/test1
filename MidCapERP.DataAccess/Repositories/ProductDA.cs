using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;

namespace MidCapERP.DataAccess.Repositories
{
    public class ProductDA : IProductDA
    {
        private readonly ISqlRepository<Product> _Product;
        private readonly CurrentUser _currentUser;

        public ProductDA(ISqlRepository<Product> product, CurrentUser currentUser)
        {
            _Product = product;
            _currentUser = currentUser;
        }

        public async Task<IQueryable<Product>> GetAll(CancellationToken cancellationToken)
        {
            return await _Product.GetAsync(cancellationToken, x => x.TenantId == _currentUser.TenantId);
        }

        public async Task<Product> GetById(Int64 Id, CancellationToken cancellationToken)
        {
            return await _Product.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<Product> CreateProduct(Product model, CancellationToken cancellationToken)
        {
            return await _Product.InsertAsync(model, cancellationToken);
        }

        public async Task<Product> UpdateProduct(Product model, CancellationToken cancellationToken)
        {
            return await _Product.UpdateAsync(model, cancellationToken);
        }

        public async Task<Product> DeleteProduct(Int64 Id, CancellationToken cancellationToken)
        {
            var entity = await _Product.GetByIdAsync(Id, cancellationToken);
            if (entity != null)
            {
                return await _Product.UpdateAsync(entity, cancellationToken);
            }
            return entity;
        }
    }
}