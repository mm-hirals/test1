using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IProductQuantitiesDA
    {
        public Task<IQueryable<ProductQuantities>> GetAll(CancellationToken cancellationToken);

        public Task<ProductQuantities> GetById(long Id, CancellationToken cancellationToken);

        public Task<ProductQuantities> UpdateProductQuantities(long Id, ProductQuantities model, CancellationToken cancellationToken);
    }
}