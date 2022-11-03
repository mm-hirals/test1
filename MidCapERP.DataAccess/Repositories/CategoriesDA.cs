using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
