using MidCapERP.CronJob.Services.Email;

namespace MidCapERP.CronJob
{
    public class SchedulerForSendGreetingsEmail : CronJobService
    {
        private readonly ISendGreetingEmailForNotification _sendGreetingEmailForNotification;

        public SchedulerForSendGreetingsEmail(ISendGreetingEmailForNotification sendGreetingEmailForNotification, IScheduleConfig<SchedulerForSendGreetingsEmail> config) : base(config.CronExpression, config.TimeZoneInfo)
        {
            _sendGreetingEmailForNotification = sendGreetingEmailForNotification;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            try
            {
                await _sendGreetingEmailForNotification.SendGreetingEmail(cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}