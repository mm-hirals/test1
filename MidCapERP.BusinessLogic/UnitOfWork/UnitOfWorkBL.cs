﻿using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataEntities;

namespace MidCapERP.BusinessLogic.UnitOfWork
{
    public class UnitOfWorkBL : IUnitOfWorkBL
    {
        private readonly ApplicationDbContext _context;
        public ICategoriesBL CategoriesBL { get; }
        public  IErrorLogsBL ErrorLogsBL { get; }

        public UnitOfWorkBL(ApplicationDbContext context, ICategoriesBL categoriesBL, IErrorLogsBL errorLogsBL)
        {
            this._context = context;
            this.CategoriesBL = categoriesBL;
            this.ErrorLogsBL = errorLogsBL;
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