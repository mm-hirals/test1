using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IErrorLogsDA
    {
        public Task<IQueryable<ErrorLogs>> GetAll(CancellationToken cancellationToken);

        public Task<ErrorLogs> GetById(int Id, CancellationToken cancellationToken);
    }
}