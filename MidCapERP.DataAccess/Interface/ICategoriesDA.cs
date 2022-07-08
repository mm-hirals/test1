using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface ICategoriesDA
    {
        public Task<IQueryable<Categories>> GetAll(CancellationToken cancellationToken);

        public Task<Categories> GetById(int Id, CancellationToken cancellationToken);

        public Task<Categories> CreateCategory(Categories model, CancellationToken cancellationToken);

        public Task<Categories> UpdateCategory(int Id, Categories model, CancellationToken cancellationToken);

        public Task<Categories> DeleteCategory(int Id, CancellationToken cancellationToken);
    }
}