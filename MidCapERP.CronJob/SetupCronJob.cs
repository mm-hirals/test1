using Microsoft.Extensions.DependencyInjection;
using MidCapERP.CronJob.Configuration;
using MidCapERP.CronJob.Services.Email;
using MidCapERP.CronJob.Services.Import_Customers;

namespace MidCapERP.CronJob
{
    public static class SetupCronJob
    {
        public static void SetupCronJobs(this IServiceCollection services)
        {
            services.AddCronJob<SchedulerForSendGreetingsEmail>(c =>
            {
                c.TimeZoneInfo = TimeZoneInfo.Local;
                c.CronExpression = @"* * * * *";
            });

            services.AddCronJob<SchedulerForImportCustomers>(c =>
            {
                c.TimeZoneInfo = TimeZoneInfo.Local;
                c.CronExpression = @"* * * * *";
            });

            services.AddSingleton<ISendGreetingEmailForNotification, SendGreetingEmailForNotification>();
            services.AddSingleton<IImportCustomers, ImportCustomers>();
        }
    }
}