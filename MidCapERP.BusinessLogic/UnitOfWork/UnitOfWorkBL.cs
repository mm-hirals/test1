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
        public ILookupsBL LookupsBL { get; }
        public IStatusesBL StatusesBL { get; }
        public IContractorsBL ContractorsBL { get; }
        public ISubjectTypesBL SubjectTypesBL { get; }
        public ILookupValuesBL LookupValuesBL { get; }
        public IContractorCategoryMappingBL ContractorCategoryMappingBL { get; }
        public ICustomersBL CustomersBL { get; }
        public UnitOfWorkBL(ApplicationDbContext context,ICategoriesBL categoriesBL, ILookupsBL lookupsBL, IStatusesBL statusesBL, IContractorsBL contractorsBL, ISubjectTypesBL subjectTypesBL, ILookupValuesBL lookupValuesBL, IContractorCategoryMappingBL contractorCategoryMapping, ICustomersBL customersBL)
        {
            this._context = context;
            this.CategoriesBL = categoriesBL;
            this.LookupsBL = lookupsBL;
            this.StatusesBL = statusesBL;
            this.ContractorsBL = contractorsBL;
            this.SubjectTypesBL = subjectTypesBL;
            this.LookupValuesBL = lookupValuesBL;
            this.ContractorCategoryMappingBL = contractorCategoryMapping;
            this.CustomersBL = customersBL;
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