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
        public IFrameDA FrameDA { get; }
        public IPolishDA PolishDA { get; }
        public ITenantDA TenantDA { get; }
        public IUserTenantMappingDA UserTenantMappingDA { get; }
        public IUserDA UserDA { get; }
        public ICustomerAddressesDA CustomerAddressesDA { get; }
        public ICustomerTypesDA CustomerTypesDA { get; }
        public IProductDA ProductDA { get; }
        public IProductImageDA ProductImageDA { get; }
        public IProductMaterialDA ProductMaterialDA { get; }
        public IRoleDA RoleDA { get; }
        public IRolePermissionDA RolePermissionDA { get; }
        public IOrderDA OrderDA { get; }
        public IOrderSetDA OrderSetDA { get; }
        public IOrderSetItemDA OrderSetItemDA { get; }

        public UnitOfWorkDA(ApplicationDbContext context, ILookupsDA lookupsDA, IContractorsDA contractorsDA, ISubjectTypesDA subjectTypesDA, ILookupValuesDA lookupValuesDA, IContractorCategoryMappingDA contractorCategoryMappingDa, ICustomersDA customersDA, IErrorLogsDA errorLogsDA, IAccessoriesTypeDA accessoriesTypesDA, IRawMaterialDA rawMaterialDA, IAccessoriesDA accessoriesDA, IFabricDA fabricDA, IFrameDA frameDA, IPolishDA polishDA, IUserTenantMappingDA userTenantMappingDA, ITenantDA tenantDA, IUserDA userDA, ICustomerAddressesDA customerAddressesDA, ICustomerTypesDA customerTypesDA, IProductDA productDA, IProductImageDA productImageDA, IProductMaterialDA productMaterialDA, IRoleDA roleDA, IRolePermissionDA rolePermissionDA, IOrderDA orderDA, IOrderSetDA orderSetDA, IOrderSetItemDA orderSetItemDA)
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
                this.FrameDA = frameDA;
                this.PolishDA = polishDA;
                this.UserTenantMappingDA = userTenantMappingDA;
                this.TenantDA = tenantDA;
                this.UserDA = userDA;
                this.CustomerAddressesDA = customerAddressesDA;
                this.CustomerTypesDA = customerTypesDA;
                this.ProductDA = productDA;
                this.ProductImageDA = productImageDA;
                this.ProductMaterialDA = productMaterialDA;
                this.RoleDA = roleDA;
                this.RolePermissionDA = rolePermissionDA;
                this.OrderDA = orderDA;
                this.OrderSetDA = orderSetDA;
                this.OrderSetItemDA = orderSetItemDA;
            }
        }

        #region TransactionMethod
        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }
        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }
        public async Task rollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }
        #endregion

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