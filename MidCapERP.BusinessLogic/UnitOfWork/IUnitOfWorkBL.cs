using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Services.ActivityLog;
using MidCapERP.BusinessLogic.Services.FileStorage;
using MidCapERP.BusinessLogic.Services.QRCodeGenerate;
using MidCapERP.BusinessLogic.Services.SendSMS;
using MidCapERP.Core.Services.Email;

namespace MidCapERP.BusinessLogic.UnitOfWork
{
    public interface IUnitOfWorkBL
    {
        IContractorsBL ContractorsBL { get; }
        ISubjectTypesBL SubjectTypesBL { get; }
        IContractorCategoryMappingBL ContractorCategoryMappingBL { get; }
        IInteriorsBL InteriorsBL { get; }
        IInteriorAddressesBL InteriorAddressesBL { get; }
        ICustomersBL CustomersBL { get; }
        IErrorLogsBL ErrorLogsBL { get; }
        ICategoryBL CategoryBL { get; }
        ICompanyBL CompanyBL { get; }
        IUnitBL UnitBL { get; }
        IRawMaterialBL RawMaterialBL { get; }
        IFabricBL FabricBL { get; }
        IPolishBL PolishBL { get; }
        IUserTenantMappingBL UserTenantMappingBL { get; }
        IUserBL UserBL { get; }
        ICustomerAddressesBL CustomerAddressesBL { get; }
        IRoleBL RoleBL { get; }
        IRolePermissionBL RolePermissionBL { get; }
        IFileStorageService FileStorageService { get; }
        IQRCodeService IQRCodeService { get; }
        IProductBL ProductBL { get; }
        ITenantBL TenantBL { get; }
        ITenantBankDetailBL TenantBankDetailBL { get; }
        IOrderBL OrderBL { get; }
        IDashboardBL DashboardBL { get; }
        IActivityLogsService ActivityLogsService { get; }
        ITenantSMTPDetailBL TenantSMTPDetailBL { get; }
        ISendSMSservice SendSMSservice { get; }
        IEmailHelper EmailHelper { get; }
        IWrkImportFilesBL WrkImportFilesBL { get; }
        IWrkImportCustomersBL WrkImportCustomersBL { get; }
        IProductQuantitiesBL ProductQuantitiesBL { get; }
        ICustomerVisitsBL CustomerVisitsBL { get; }
    }
}