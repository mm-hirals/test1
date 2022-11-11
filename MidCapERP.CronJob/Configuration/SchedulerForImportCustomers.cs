using MidCapERP.CronJob.Services.Import_Customers;

namespace MidCapERP.CronJob.Configuration
{
    public class SchedulerForImportCustomers : CronJobService
    {
        private readonly IImportCustomers _importCustomers;

        public SchedulerForImportCustomers(IImportCustomers importCustomers, IScheduleConfig<SchedulerForImportCustomers> config) : base(config.CronExpression, config.TimeZoneInfo)
        {
            _importCustomers = importCustomers;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            //await _importCustomers.ImportCustomersFromCsvFile(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }
    }
}