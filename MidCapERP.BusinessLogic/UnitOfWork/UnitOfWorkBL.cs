using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Services.ActivityLog;
using MidCapERP.BusinessLogic.Services.FileStorage;
using MidCapERP.BusinessLogic.Services.QRCodeGenerate;
using MidCapERP.BusinessLogic.Services.SendSMS;
using MidCapERP.Core.Services.Email;
using MidCapERP.DataEntities;

namespace MidCapERP.BusinessLogic.UnitOfWork
{
    public class UnitOfWorkBL : IUnitOfWorkBL
    {
        private readonly ApplicationDbContext _context;
        public IContractorsBL ContractorsBL { get; }
        public ISubjectTypesBL SubjectTypesBL { get; }
        public IContractorCategoryMappingBL ContractorCategoryMappingBL { get; }
        public IInteriorsBL InteriorsBL { get; }
        public ICustomersBL CustomersBL { get; }
        public IErrorLogsBL ErrorLogsBL { get; }
        public ICategoryBL CategoryBL { get; }
        public ICompanyBL CompanyBL { get; }
        public IUnitBL UnitBL { get; }
        public IRawMaterialBL RawMaterialBL { get; }
        public IFileStorageService FileStorageService { get; }
        public IQRCodeService IQRCodeService { get; }
        public IFabricBL FabricBL { get; }
        public IPolishBL PolishBL { get; }
        public IUserTenantMappingBL UserTenantMappingBL { get; }
        public IUserBL UserBL { get; }
        public IInteriorAddressesBL InteriorAddressesBL { get; }
        public ICustomerAddressesBL CustomerAddressesBL { get; }
        public IProductBL ProductBL { get; }
        public IRoleBL RoleBL { get; }
        public IRolePermissionBL RolePermissionBL { get; }
        public IOrderBL OrderBL { get; }
        public IDashboardBL DashboardBL { get; }
        public ITenantBL TenantBL { get; }
        public ITenantBankDetailBL TenantBankDetailBL { get; }
        public IActivityLogsService ActivityLogsService { get; }
        public ITenantSMTPDetailBL TenantSMTPDetailBL { get; }
        public ISendSMSservice SendSMSservice { get; }
        public IEmailHelper EmailHelper { get; }
        public IWrkImportCustomersBL WrkImportCustomersBL { get; }
        public IWrkImportFilesBL WrkImportFilesBL { get; }
        public IProductQuantitiesBL ProductQuantitiesBL { get; }

        public ICustomerVisitsBL CustomerVisitsBL { get; }

        public UnitOfWorkBL(ApplicationDbContext context, IContractorsBL contractorsBL, ISubjectTypesBL subjectTypesBL, IContractorCategoryMappingBL contractorCategoryMapping, IInteriorsBL interiorsBL, ICustomersBL customersBL, IErrorLogsBL errorLogsBL, ICategoryBL categoryBL, ICompanyBL companyBL, IUnitBL unitBL, IRawMaterialBL rawMaterialBL, IFileStorageService fileStorageService, IQRCodeService iQRCodeService, IFabricBL fabricBL, IPolishBL polishBL, IUserTenantMappingBL userTenantMappingBL, IUserBL userBL, IProductBL productBL, IInteriorAddressesBL interiorAddressesBL, ICustomerAddressesBL customerAddressesBL, IRoleBL roleBL, IRolePermissionBL rolePermissionBL, IOrderBL orderBL, IDashboardBL dashboardBL, ITenantBL tenantBL, ITenantBankDetailBL tenantBankDetailBL, IActivityLogsService activityLogsService, ITenantSMTPDetailBL tenantSMTPDetailBL, ISendSMSservice sendSMSservice, IEmailHelper emailHelper, IWrkImportCustomersBL wrkImportCustomersBL, IWrkImportFilesBL wrkImportFilesBL, IProductQuantitiesBL productQuantitiesBL, ICustomerVisitsBL customerVisitsBL)
        {
            _context = context;
            ContractorsBL = contractorsBL;
            SubjectTypesBL = subjectTypesBL;
            ContractorCategoryMappingBL = contractorCategoryMapping;
            InteriorsBL = interiorsBL;
            CustomersBL = customersBL;
            ErrorLogsBL = errorLogsBL;
            CategoryBL = categoryBL;
            CompanyBL = companyBL;
            UnitBL = unitBL;
            RawMaterialBL = rawMaterialBL;
            FileStorageService = fileStorageService;
            IQRCodeService = iQRCodeService;
            FabricBL = fabricBL;
            PolishBL = polishBL;
            UserTenantMappingBL = userTenantMappingBL;
            UserBL = userBL;
            CustomerAddressesBL = customerAddressesBL;
            ProductBL = productBL;
            InteriorAddressesBL = interiorAddressesBL;
            RoleBL = roleBL;
            RolePermissionBL = rolePermissionBL;
            OrderBL = orderBL;
            DashboardBL = dashboardBL;
            TenantBL = tenantBL;
            TenantBankDetailBL = tenantBankDetailBL;
            ActivityLogsService = activityLogsService;
            TenantSMTPDetailBL = tenantSMTPDetailBL;
            SendSMSservice = sendSMSservice;
            EmailHelper = emailHelper;
            WrkImportCustomersBL = wrkImportCustomersBL;
            WrkImportFilesBL = wrkImportFilesBL;
            ProductQuantitiesBL = productQuantitiesBL;
            CustomerVisitsBL = customerVisitsBL;
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