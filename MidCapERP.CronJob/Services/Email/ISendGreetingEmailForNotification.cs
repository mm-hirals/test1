namespace MidCapERP.CronJob.Services.Email
{
    public interface ISendGreetingEmailForNotification
    {
        public Task SendGreetingEmail(CancellationToken cancellationToken);
    }
}