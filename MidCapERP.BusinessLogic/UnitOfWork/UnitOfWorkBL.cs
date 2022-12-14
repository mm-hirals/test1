using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Services.FileStorage;
using MidCapERP.BusinessLogic.Services.QRCodeGenerate;
using MidCapERP.DataEntities;

namespace MidCapERP.BusinessLogic.UnitOfWork
{
    public class UnitOfWorkBL : IUnitOfWorkBL
    {
        private readonly ApplicationDbContext _context;
        public IContractorsBL ContractorsBL { get; }
        public ISubjectTypesBL SubjectTypesBL { get; }
        public IContractorCategoryMappingBL ContractorCategoryMappingBL { get; }
        public ICustomersBL CustomersBL { get; }
        public IErrorLogsBL ErrorLogsBL { get; }
        public ICategoryBL CategoryBL { get; }
        public ICompanyBL CompanyBL { get; }
        public IUnitBL UnitBL { get; }
        public IFrameTypeBL FrameTypeBL { get; }
        public IAccessoriesTypeBL AccessoriesTypeBL { get; }
        public IRawMaterialBL RawMaterialBL { get; }
        public IFileStorageService FileStorageService { get; }
        public IQRCodeService IQRCodeService { get; }
        public IAccessoriesBL AccessoriesBL { get; }
        public IFabricBL FabricBL { get; }
        public IFrameBL FrameBL { get; }
        public IPolishBL PolishBL { get; }
        public IUserTenantMappingBL UserTenantMappingBL { get; }
        public IUserBL UserBL { get; }

        public ICustomerAddressesBL CustomerAddressesBL { get; }
        public IProductBL ProductBL { get; }
        public IRoleBL RoleBL { get; }
        public IRolePermissionBL RolePermissionBL { get; }
        public IOrderBL OrderBL { get; }
        public IDashboardBL DashboardBL { get; }

        public UnitOfWorkBL(ApplicationDbContext context, IContractorsBL contractorsBL, ISubjectTypesBL subjectTypesBL, IContractorCategoryMappingBL contractorCategoryMapping, ICustomersBL customersBL, IErrorLogsBL errorLogsBL, ICategoryBL categoryBL, ICompanyBL companyBL, IUnitBL unitBL, IFrameTypeBL frameTypeBL, IAccessoriesTypeBL accessoriesTypesBL, IRawMaterialBL rawMaterialBL, IAccessoriesBL accessoriesBL, IFileStorageService fileStorageService, IQRCodeService iQRCodeService, IFabricBL fabricBL, IFrameBL frameBL, IPolishBL polishBL, IUserTenantMappingBL userTenantMappingBL, IUserBL userBL, IProductBL productBL, ICustomerAddressesBL customerAddressesBL, IRoleBL roleBL, IRolePermissionBL rolePermissionBL, IOrderBL orderBL, IDashboardBL dashboardBL)
        {
            this._context = context;
            this.ContractorsBL = contractorsBL;
            this.SubjectTypesBL = subjectTypesBL;
            this.ContractorCategoryMappingBL = contractorCategoryMapping;
            this.CustomersBL = customersBL;
            this.ErrorLogsBL = errorLogsBL;
            this.CategoryBL = categoryBL;
            this.CompanyBL = companyBL;
            this.UnitBL = unitBL;
            this.FrameTypeBL = frameTypeBL;
            this.AccessoriesTypeBL = accessoriesTypesBL;
            this.RawMaterialBL = rawMaterialBL;
            this.FileStorageService = fileStorageService;
            this.IQRCodeService = iQRCodeService;
            this.AccessoriesBL = accessoriesBL;
            this.FabricBL = fabricBL;
            this.FrameBL = frameBL;
            this.PolishBL = polishBL;
            this.UserTenantMappingBL = userTenantMappingBL;
            this.UserBL = userBL;
            this.CustomerAddressesBL = customerAddressesBL;
            this.ProductBL = productBL;
            this.RoleBL = roleBL;
            this.RolePermissionBL = rolePermissionBL;
            this.OrderBL = orderBL;
            this.DashboardBL = dashboardBL;
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