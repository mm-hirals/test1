using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;

namespace MidCapERP.DataAccess.Repositories
{
    public class CategoriesDA : ICategoriesDA
    {
        private readonly ISqlRepository<Categories> _categories;
        private readonly CurrentUser _currentUser;

        public CategoriesDA(ISqlRepository<Categories> categories, CurrentUser currentUser)
        {
            _categories = categories;
            _currentUser = currentUser;
        }

        public async Task<Categories> CreateCategory(Categories model, CancellationToken cancellationToken)
        {
            return await _categories.InsertAsync(model, cancellationToken);
        }

        public async Task<IQueryable<Categories>> GetAll(CancellationToken cancellationToken)
        {
            return await _categories.GetAsync(cancellationToken, x => x.IsDeleted == false);
        }
    }
}
