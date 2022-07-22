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
            services.AddScoped<ICategoriesDA, CategoriesDA>();
            services.AddScoped<IContractorCategoryMappingDA, ContractorCategoryMappingDA>();

            // KEEP THIS LINE AT THE END.
            services.AddScoped<IUnitOfWorkDA, UnitOfWorkDA>();
        }
    }
}