using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataEntities;

namespace MidCapERP.BusinessLogic.UnitOfWork
{
    public class UnitOfWorkBL : IUnitOfWorkBL
    {
        private readonly ApplicationDbContext _context;
        public ILookupsBL LookupsBL { get; }
        public IContractorsBL ContractorsBL { get; }
        public ISubjectTypesBL SubjectTypesBL { get; }
        public ILookupValuesBL LookupValuesBL { get; }
        public IContractorCategoryMappingBL ContractorCategoryMappingBL { get; }
        public ICustomersBL CustomersBL { get; }
        public IErrorLogsBL ErrorLogsBL { get; }
        public ICategoryBL CategoryBL { get; }
        public ICompanyBL CompanyBL { get; }
        public IUnitBL UnitBL { get; }
        public IWoodTypeBL WoodTypeBL { get; }

        public UnitOfWorkBL(ApplicationDbContext context, ILookupsBL lookupsBL, IContractorsBL contractorsBL, ISubjectTypesBL subjectTypesBL, ILookupValuesBL lookupValuesBL, IContractorCategoryMappingBL contractorCategoryMapping, ICustomersBL customersBL, IErrorLogsBL errorLogsBL, ICategoryBL categoryBL, ICompanyBL companyBL, IUnitBL unitBL, IWoodTypeBL woodTypeBL)
        {
            this._context = context;
            this.LookupsBL = lookupsBL;
            this.ContractorsBL = contractorsBL;
            this.SubjectTypesBL = subjectTypesBL;
            this.LookupValuesBL = lookupValuesBL;
            this.ContractorCategoryMappingBL = contractorCategoryMapping;
            this.CustomersBL = customersBL;
            this.ErrorLogsBL = errorLogsBL;
            this.CategoryBL = categoryBL;
            this.CompanyBL = companyBL;
            this.UnitBL = unitBL;
            this.WoodTypeBL = woodTypeBL;
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