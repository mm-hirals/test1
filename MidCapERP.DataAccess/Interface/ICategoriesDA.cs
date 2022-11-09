using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface ICategoriesDA
    {
        public Task<IQueryable<Categories>> GetAll(CancellationToken cancellationToken);

        public Task<Categories> GetById(long Id, CancellationToken cancellationToken);

        public Task<Categories> CreateCategory(Categories model, CancellationToken cancellationToken);

        public Task<Categories> UpdateCategory(long Id, Categories model, CancellationToken cancellationToken);
    }
}