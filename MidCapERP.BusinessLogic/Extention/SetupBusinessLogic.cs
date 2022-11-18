using Microsoft.Extensions.DependencyInjection;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Repositories;
using MidCapERP.BusinessLogic.Services.ActivityLog;
using MidCapERP.BusinessLogic.Services.FileStorage;
using MidCapERP.BusinessLogic.Services.QRCodeGenerate;
using MidCapERP.BusinessLogic.Services.SendSMS;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Services.Email;

namespace MidCapERP.BusinessLogic.Extention
{
    public static class SetupBusinessLogic
    {
        public static void SetupUnitOfWorkBL(this IServiceCollection services)
        {
            services.AddScoped<IContractorsBL, ContractorsBL>();
            services.AddScoped<ISubjectTypesBL, SubjectTypesBL>();
            services.AddScoped<IContractorCategoryMappingBL, ContractorCategoryMappingBL>();
            services.AddScoped<IInteriorsBL, InteriorsBL>();
            services.AddScoped<ICustomersBL, CustomersBL>();
            services.AddScoped<IErrorLogsBL, ErrorLogsBL>();
            services.AddScoped<ICategoryBL, CategoryBL>();
            services.AddScoped<ICompanyBL, CompanyBL>();
            services.AddScoped<IUnitBL, UnitBL>();
            services.AddScoped<IRawMaterialBL, RawMaterialBL>();
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IQRCodeService, QRCodeService>();
            services.AddScoped<IFabricBL, FabricBL>();
            services.AddScoped<IPolishBL, PolishBL>();
            services.AddScoped<IUserTenantMappingBL, UserTenantMappingBL>();
            services.AddScoped<IUserBL, UserBL>();
            services.AddScoped<IInteriorAddressesBL, InteriorAddressesBL>();
            services.AddScoped<ICustomerAddressesBL, CustomerAddressesBL>();
            services.AddScoped<IProductBL, ProductBL>();
            services.AddScoped<IRoleBL, RoleBL>();
            services.AddScoped<IRolePermissionBL, RolePermissionBL>();
            services.AddScoped<ITenantBL, TenantBL>();
            services.AddScoped<ITenantBankDetailBL, TenantBankDetailBL>();
            services.AddScoped<IOrderBL, OrderBL>();
            services.AddScoped<IDashboardBL, DashboardBL>();
            services.AddScoped<IActivityLogsService, ActivityLogsService>();
            services.AddScoped<ITenantSMTPDetailBL, TenantSMTPDetailBL>();
            services.AddScoped<ISendSMSservice, SendSMSservice>();
            services.AddScoped<IEmailHelper, EmailHelper>();
            services.AddScoped<IWrkImportFilesBL, WrkImportFilesBL>();
            services.AddScoped<IWrkImportCustomersBL, WrkImportCustomersBL>();
            services.AddScoped<IProductQuantitiesBL, ProductQuantitiesBL>();
            services.AddScoped<ICustomerVisitsBL, CustomerVisitsBL>();
            //KEEP THIS LINE AT THE BOTTOM
            services.AddScoped<IUnitOfWorkBL, UnitOfWorkBL>();
        }

        public static void SetupAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}