﻿using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities;

namespace MidCapERP.DataAccess.UnitOfWork
{
    public class UnitOfWorkDA : IUnitOfWorkDA
    {
        private readonly ApplicationDbContext _context;
        public ICategoriesDA CategoriesDA { get; }
        public ILookupValuesDA LookupValuesDA { get; }

        public UnitOfWorkDA(ApplicationDbContext context, ICategoriesDA categoriesDA, ILookupValuesDA lookupValuesDA)
        {
            this._context = context;
            this.CategoriesDA = categoriesDA;
            this.LookupValuesDA = lookupValuesDA;
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