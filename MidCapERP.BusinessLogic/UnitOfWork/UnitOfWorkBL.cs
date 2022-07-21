using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.BusinessLogic.UnitOfWork
{
    public class UnitOfWorkBL : IUnitOfWorkBL
    {
        private readonly ApplicationDbContext _context;
        public ICategoriesBL CategoriesBL { get; }
        public ISubjectTypesBL SubjectTypesBL { get; }

        public UnitOfWorkBL(ICategoriesBL categoriesBL, ApplicationDbContext context,  ISubjectTypesBL subjectTypesBL)
        {
            this._context = context;
            this.CategoriesBL = categoriesBL;
            this.SubjectTypesBL = subjectTypesBL;
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