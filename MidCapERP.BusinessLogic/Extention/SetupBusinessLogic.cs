using Microsoft.Extensions.DependencyInjection;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Repositories;
using MidCapERP.BusinessLogic.Services.FileStorage;
using MidCapERP.BusinessLogic.Services.QRCodeGenerate;
using MidCapERP.BusinessLogic.UnitOfWork;

namespace MidCapERP.BusinessLogic.Extention
{
    public static class SetupBusinessLogic
    {
        public static void SetupUnitOfWorkBL(this IServiceCollection services)
        {
            services.AddScoped<IContractorsBL, ContractorsBL>();
            services.AddScoped<ISubjectTypesBL, SubjectTypesBL>();
            services.AddScoped<IContractorCategoryMappingBL, ContractorCategoryMappingBL>();
            services.AddScoped<ICustomersBL, CustomersBL>();
            services.AddScoped<IErrorLogsBL, ErrorLogsBL>();
            services.AddScoped<ICategoryBL, CategoryBL>();
            services.AddScoped<ICompanyBL, CompanyBL>();
            services.AddScoped<IUnitBL, UnitBL>();
            services.AddScoped<IFrameTypeBL, FrameTypeBL>();
            services.AddScoped<IAccessoriesTypeBL, AccessoriesTypeBL>();
            services.AddScoped<IRawMaterialBL, RawMaterialBL>();
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IQRCodeService, QRCodeService>();
            services.AddScoped<IAccessoriesBL, AccessoriesBL>();
            services.AddScoped<IFabricBL, FabricBL>();
            services.AddScoped<IFrameBL, FrameBL>();
            services.AddScoped<IPolishBL, PolishBL>();
            services.AddScoped<IUserTenantMappingBL, UserTenantMappingBL>();
            services.AddScoped<IUserBL, UserBL>();
            services.AddScoped<ICustomerAddressesBL, CustomerAddressesBL>();
            services.AddScoped<IProductBL, ProductBL>();
            services.AddScoped<IRoleBL, RoleBL>();
            services.AddScoped<IRolePermissionBL, RolePermissionBL>();
            services.AddScoped<IOrderBL, OrderBL>();

            //KEEP THIS LINE AT THE BOTTOM
            services.AddScoped<IUnitOfWorkBL, UnitOfWorkBL>();
        }

        public static void SetupAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}