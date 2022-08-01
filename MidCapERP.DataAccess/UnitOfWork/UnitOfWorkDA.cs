using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities;

namespace MidCapERP.DataAccess.UnitOfWork
{
    public class UnitOfWorkDA : IUnitOfWorkDA
    {
        private readonly ApplicationDbContext _context;
        public IContractorsDA ContractorsDA { get; }
        public ILookupValuesDA LookupValuesDA { get; }
        public ILookupsDA LookupsDA { get; }
        public IStatusDA StatusDA { get; }
        public ISubjectTypesDA SubjectTypesDA { get; }
        public IContractorCategoryMappingDA ContractorCategoryMappingDA { get; }
        public ICustomersDA CustomersDA { get; }
        public IErrorLogsDA ErrorLogsDA { get; }
        public IAccessoriesTypesDA AccessoriesTypesDA { get; }


        public UnitOfWorkDA(ApplicationDbContext context, ILookupsDA lookupsDA, IStatusDA statusBL, IContractorsDA contractorsDA, ISubjectTypesDA subjectTypesDA, ILookupValuesDA lookupValuesDA, IContractorCategoryMappingDA contractorCategoryMappingDa, ICustomersDA customersDA, IErrorLogsDA errorLogsDA, IAccessoriesTypesDA accessoriesTypesDA)
        {
            this._context = context;
            this.ContractorsDA = contractorsDA;
            this.LookupsDA = lookupsDA;
            this.StatusDA = statusBL;
            this.SubjectTypesDA = subjectTypesDA;
            this.LookupValuesDA = lookupValuesDA;
            this.ContractorCategoryMappingDA = contractorCategoryMappingDa;
            this.CustomersDA = customersDA;
            this.ErrorLogsDA = errorLogsDA;
            this.AccessoriesTypesDA = accessoriesTypesDA;
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