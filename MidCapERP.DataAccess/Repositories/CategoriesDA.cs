using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class CategoriesDA : ICategoriesDA
    {
        private readonly ISqlRepository<Categories> _categories;

        public CategoriesDA(ISqlRepository<Categories> categories)
        {
            _categories = categories;
        }

        public async Task<IQueryable<Categories>> GetAll(CancellationToken cancellationToken)
        {
            return await _categories.GetAsync(cancellationToken);
        }

        public async Task<Categories> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _categories.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<Categories> CreateCategory(Categories model, CancellationToken cancellationToken)
        {
            return await _categories.InsertAsync(model, cancellationToken);
        }

        public async Task<Categories> UpdateCategory(int Id, Categories model, CancellationToken cancellationToken)
        {
            return await _categories.UpdateAsync(model, cancellationToken);
        }

        public async Task<Categories> DeleteCategory(int Id, CancellationToken cancellationToken)
        {
            var entity = await _categories.GetByIdAsync(Id, cancellationToken);
            if (entity != null)
            {
                return await _categories.UpdateAsync(entity, cancellationToken);
            }
            return entity;
        }
    }
}