using MidCapERP.DataEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidCapERP.DataAccess.Interface
{
    public interface ICategoriesDA
    {
        public Task<Categories> CreateCategory(Categories model, CancellationToken cancellationToken);
        public Task<IQueryable<Categories>> GetAll(CancellationToken cancellationToken);


    }
}
