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
            services.AddScoped<ICategoriesBL, CategoriesBL>();
            services.AddScoped<IContractorsBL, ContractorsBL>();

            //KEEP THIS LINE AT THE BOTTOM
            services.AddScoped<IUnitOfWorkBL, UnitOfWorkBL>();
        }

        public static void SetupAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}