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
        public ISubjectTypesDA SubjectTypesDA { get; }
        public IContractorCategoryMappingDA ContractorCategoryMappingDA { get; }
        public ICustomersDA CustomersDA { get; }
        public IErrorLogsDA ErrorLogsDA { get; }
        public IAccessoriesTypeDA AccessoriesTypeDA { get; }
        public IRawMaterialDA RawMaterialDA { get; }
        public IAccessoriesDA AccessoriesDA { get; }
        public IFabricDA FabricDA { get; }
        public IWoodDA WoodDA { get; }
        public IPolishDA PolishDA { get; }
        public ITenantDA TenantDA { get; }
        public IUserTenantMappingDA UserTenantMappingDA { get; }

        public UnitOfWorkDA(ApplicationDbContext context, ILookupsDA lookupsDA, IContractorsDA contractorsDA, ISubjectTypesDA subjectTypesDA, ILookupValuesDA lookupValuesDA, IContractorCategoryMappingDA contractorCategoryMappingDa, ICustomersDA customersDA, IErrorLogsDA errorLogsDA, IAccessoriesTypeDA accessoriesTypesDA, IRawMaterialDA rawMaterialDA, IAccessoriesDA accessoriesDA, IFabricDA fabricDA, IWoodDA woodDA, IPolishDA polishDA, IUserTenantMappingDA userTenantMappingDA, ITenantDA tenantDA)
        {
            {
                this._context = context;
                this.ContractorsDA = contractorsDA;
                this.LookupsDA = lookupsDA;
                this.SubjectTypesDA = subjectTypesDA;
                this.LookupValuesDA = lookupValuesDA;
                this.ContractorCategoryMappingDA = contractorCategoryMappingDa;
                this.CustomersDA = customersDA;
                this.ErrorLogsDA = errorLogsDA;
                this.AccessoriesTypeDA = accessoriesTypesDA;
                this.RawMaterialDA = rawMaterialDA;
                this.AccessoriesDA = accessoriesDA;
                this.FabricDA = fabricDA;
                this.WoodDA = woodDA;
                this.PolishDA = polishDA;
                this.UserTenantMappingDA = userTenantMappingDA;
                this.TenantDA = tenantDA;
            }
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