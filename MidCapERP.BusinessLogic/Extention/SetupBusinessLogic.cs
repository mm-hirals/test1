using Microsoft.Extensions.DependencyInjection;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Repositories;
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
            services.AddScoped<IWoodTypeBL, WoodTypeBL>();
            services.AddScoped<IAccessoriesTypesBL, AccessoriesTypesBL>();
            services.AddScoped<IAccessoriesBL, AccessoriesBL>();

            //KEEP THIS LINE AT THE BOTTOM
            services.AddScoped<IUnitOfWorkBL, UnitOfWorkBL>();
        }

        public static void SetupAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}