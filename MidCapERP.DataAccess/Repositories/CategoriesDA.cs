using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using static MidCapERP.Core.Constants.ApplicationIdentityConstants.Permissions;

namespace MidCapERP.DataAccess.Repositories
{
    public class CategoriesDA : ICategoriesDA
    {
        private readonly ISqlRepository<Categories> _categories;

        public CategoriesDA(ISqlRepository<Categories> categories)
        {
            _categories = categories;
        }

        public async Task<Categories> CreateCategory(Categories model, CancellationToken cancellationToken)
        {
            return await _categories.InsertAsync(model, cancellationToken);
        }

        public async Task<IQueryable<Categories>> GetAll(CancellationToken cancellationToken)
        {
            return await _categories.GetAsync(cancellationToken, x => x.IsDeleted == false);
        }

        public async Task<Categories> GetById(long Id, CancellationToken cancellationToken)
        {
            return await _categories.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<Categories> UpdateCategory(long Id, Categories model, CancellationToken cancellationToken)
        {
            return await _categories.UpdateAsync(model, cancellationToken);
        }
    }
}
