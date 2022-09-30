using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Services.ActivityLog;
using MidCapERP.BusinessLogic.Services.FileStorage;
using MidCapERP.BusinessLogic.Services.QRCodeGenerate;

namespace MidCapERP.BusinessLogic.UnitOfWork
{
    public interface IUnitOfWorkBL
    {
        IContractorsBL ContractorsBL { get; }
        ISubjectTypesBL SubjectTypesBL { get; }
        IContractorCategoryMappingBL ContractorCategoryMappingBL { get; }
        IArchitectsBL ArchitectsBL { get; }
        IArchitectAddressesBL ArchitectAddressesBL { get; }
        ICustomersBL CustomersBL { get; }
        IErrorLogsBL ErrorLogsBL { get; }
        ICategoryBL CategoryBL { get; }
        ICompanyBL CompanyBL { get; }
        IUnitBL UnitBL { get; }
        IFrameTypeBL FrameTypeBL { get; }
        IAccessoriesTypeBL AccessoriesTypeBL { get; }
        IRawMaterialBL RawMaterialBL { get; }
        IAccessoriesBL AccessoriesBL { get; }
        IFabricBL FabricBL { get; }
        IFrameBL FrameBL { get; }
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
    }
}