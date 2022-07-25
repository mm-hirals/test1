using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities;

namespace MidCapERP.DataAccess.UnitOfWork
{
    public class UnitOfWorkDA : IUnitOfWorkDA
    {
        private readonly ApplicationDbContext _context;
        public ICategoriesDA CategoriesDA { get; }

        public ILookupsDA LookupsDA { get; }
        public IStatusesDA StatusesDA { get; }

        public UnitOfWorkDA(ApplicationDbContext context, ICategoriesDA categoriesDA,ILookupsDA lookupsDA, IStatusesDA statusesBL)
        {
            this._context = context;
            this.CategoriesDA = categoriesDA;
            this.LookupsDA = lookupsDA;
            this.StatusesDA = statusesBL;
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