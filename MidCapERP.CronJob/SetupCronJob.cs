using Microsoft.Extensions.DependencyInjection;
using MidCapERP.CronJob.Services.Email;

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
            services.AddSingleton<ISendGreetingEmailForNotification, SendGreetingEmailForNotification>();
        }
    }
}