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
        public IRawMaterialDA RawMaterialDA { get; }
        public IFabricDA FabricDA { get; }
        public IPolishDA PolishDA { get; }
        public ITenantDA TenantDA { get; }
        public IUserTenantMappingDA UserTenantMappingDA { get; }
        public IUserDA UserDA { get; }
        public ICustomerAddressesDA CustomerAddressesDA { get; }
        public ICustomerTypesDA CustomerTypesDA { get; }
        public IProductDA ProductDA { get; }
        public IProductImageDA ProductImageDA { get; }
        public IProductMaterialDA ProductMaterialDA { get; }
        public ICategoriesDA CategoriesDA { get; }
        public IRoleDA RoleDA { get; }
        public IRolePermissionDA RolePermissionDA { get; }
        public IOrderDA OrderDA { get; }
        public ITenantBankDetailDA TenantBankDetailDA { get; }
        public IOrderSetDA OrderSetDA { get; }
        public IOrderSetItemDA OrderSetItemDA { get; }
        public IOrderSetItemImageDA OrderSetItemImageDA { get; }
        public IOrderSetItemReceivableDA OrderSetItemReceivableDA { get; }
        public IActivityLogsDA ActivityLogsDA { get; }
        public IOrderAddressDA OrderAddressDA { get; }
        public ITenantSMTPDetailDA TenantSMTPDetailDA { get; }
        public INotificationManagementDA NotificationManagementDA { get; }
        public IWrkImportFilesDA WrkImportFilesDA { get; }
        public IWrkImportCustomersDA WrkImportCustomersDA { get; }

        public UnitOfWorkDA(ApplicationDbContext context, ILookupsDA lookupsDA, IContractorsDA contractorsDA, ISubjectTypesDA subjectTypesDA, ILookupValuesDA lookupValuesDA, IContractorCategoryMappingDA contractorCategoryMappingDa, ICustomersDA customersDA, IErrorLogsDA errorLogsDA, IRawMaterialDA rawMaterialDA, IFabricDA fabricDA, IPolishDA polishDA, IUserTenantMappingDA userTenantMappingDA, ITenantDA tenantDA, IUserDA userDA, ICustomerAddressesDA customerAddressesDA, ICustomerTypesDA customerTypesDA, IProductDA productDA, IProductImageDA productImageDA, IProductMaterialDA productMaterialDA, ICategoriesDA categoriesDA, IRoleDA roleDA, IRolePermissionDA rolePermissionDA, IOrderDA orderDA, ITenantBankDetailDA tenantBankDetailDA, IOrderSetDA orderSetDA, IOrderSetItemDA orderSetItemDA, IActivityLogsDA activityLogsDA, IOrderAddressDA orderAddressDA, ITenantSMTPDetailDA tenantSMTPDetailDA, INotificationManagementDA notificationManagementDA, IOrderSetItemImageDA orderSetItemImageDA, IOrderSetItemReceivableDA orderSetItemReceivableDA, IWrkImportFilesDA wrkImportFilesDA, IWrkImportCustomersDA wrkImportCustomersDA)
        {
            _context = context;
            ContractorsDA = contractorsDA;
            LookupsDA = lookupsDA;
            SubjectTypesDA = subjectTypesDA;
            LookupValuesDA = lookupValuesDA;
            ContractorCategoryMappingDA = contractorCategoryMappingDa;
            CustomersDA = customersDA;
            ErrorLogsDA = errorLogsDA;
            RawMaterialDA = rawMaterialDA;
            FabricDA = fabricDA;
            PolishDA = polishDA;
            UserTenantMappingDA = userTenantMappingDA;
            TenantDA = tenantDA;
            UserDA = userDA;
            CustomerAddressesDA = customerAddressesDA;
            CustomerTypesDA = customerTypesDA;
            ProductDA = productDA;
            ProductImageDA = productImageDA;
            ProductMaterialDA = productMaterialDA;
            CategoriesDA = categoriesDA;
            RoleDA = roleDA;
            RolePermissionDA = rolePermissionDA;
            OrderDA = orderDA;
            TenantBankDetailDA = tenantBankDetailDA;
            OrderSetDA = orderSetDA;
            OrderSetItemDA = orderSetItemDA;
            ActivityLogsDA = activityLogsDA;
            OrderAddressDA = orderAddressDA;
            TenantSMTPDetailDA = tenantSMTPDetailDA;
            NotificationManagementDA = notificationManagementDA;
            OrderSetItemImageDA = orderSetItemImageDA;
            OrderSetItemReceivableDA = orderSetItemReceivableDA;
            WrkImportFilesDA = wrkImportFilesDA;
            WrkImportCustomersDA = wrkImportCustomersDA;
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

        #endregion TransactionMethod

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