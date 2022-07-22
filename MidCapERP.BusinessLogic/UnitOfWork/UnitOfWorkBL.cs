using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataEntities;

namespace MidCapERP.BusinessLogic.UnitOfWork
{
    public class UnitOfWorkBL : IUnitOfWorkBL
    {
        private readonly ApplicationDbContext _context;
        public ICategoriesBL CategoriesBL { get; }
        public IContractorsBL ContractorsBL { get; }

        public UnitOfWorkBL(ApplicationDbContext context, ICategoriesBL categoriesBL, IContractorsBL contractorsBL)
        {
            this._context = context;
            this.CategoriesBL = categoriesBL;
            this.ContractorsBL = contractorsBL;
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