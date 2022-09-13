using Microsoft.Extensions.DependencyInjection;
using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataAccess.Repositories;
using MidCapERP.DataAccess.UnitOfWork;

namespace MidCapERP.DataAccess.Extention
{
    public static class SetupUnitOfWork
    {
        public static void SetupUnitOfWorkDA(this IServiceCollection services)
        {
            services.AddScoped(typeof(ISqlRepository<>), typeof(SqlDBRepository<>));
            services.AddScoped<ILookupsDA, LookupsDA>();
            services.AddScoped<IContractorsDA, ContractorsDA>();
            services.AddScoped<ISubjectTypesDA, SubjectTypesDA>();
            services.AddScoped<ILookupValuesDA, LookupValuesDA>();
            services.AddScoped<IContractorCategoryMappingDA, ContractorCategoryMappingDA>();
            services.AddScoped<ICustomersDA, CustomersDA>();
            services.AddScoped<IErrorLogsDA, ErrorLogsDA>();
            services.AddScoped<IAccessoriesTypeDA, AccessoriesTypeDA>();
            services.AddScoped<IRawMaterialDA, RawMaterialDA>();
            services.AddScoped<IAccessoriesDA, AccessoriesDA>();
            services.AddScoped<IFabricDA, FabricDA>();
            services.AddScoped<IFrameDA, FrameDA>();
            services.AddScoped<IPolishDA, PolishDA>();
            services.AddScoped<ITenantDA, TenantDA>();
            services.AddScoped<IUserTenantMappingDA, UserTenantMappingDA>();
            services.AddScoped<IUserDA, UserDA>();
            services.AddScoped<ICustomerAddressesDA, CustomerAddressesDA>();
            services.AddScoped<ICustomerTypesDA, CustomerTypesDA>();
            services.AddScoped<IProductDA, ProductDA>();
            services.AddScoped<IProductImageDA, ProductImageDA>();
            services.AddScoped<IProductMaterialDA, ProductMaterialDA>();
            services.AddScoped<IRoleDA, RoleDA>();
            services.AddScoped<IRolePermissionDA, RolePermissionDA>();
            services.AddScoped<IOTPLoginDA, LoginTokenDA>();
            // KEEP THIS LINE AT THE END.
            services.AddScoped<IUnitOfWorkDA, UnitOfWorkDA>();
        }
    }
}