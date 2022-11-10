namespace MidCapERP.CronJob.Services.Import_Customers
{
    public interface IImportCustomers
    {
        public Task ImportCustomersFromCsvFile(CancellationToken cancellationToken);
    }
}