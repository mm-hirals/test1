using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities;

namespace MidCapERP.DataAccess.UnitOfWork
{
    public class UnitOfWorkDA : IUnitOfWorkDA
    {
        private readonly ApplicationDbContext _context;
        public ICategoriesDA CategoriesDA { get; }
        public ICustomersDA CustomersDA { get; }

        public UnitOfWorkDA(ApplicationDbContext context, ICategoriesDA categoriesDA, ICustomersDA customersDA)
        {
            this._context = context;
            this.CategoriesDA = categoriesDA;
            this.CustomersDA = customersDA;
        }

        #region DisposeMethod

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion DisposeMethod
    }
}