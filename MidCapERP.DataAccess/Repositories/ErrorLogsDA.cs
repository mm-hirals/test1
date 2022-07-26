using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class ErrorLogsDA : IErrorLogsDA
    {
        private readonly ISqlRepository<ErrorLogs> _errorLogs;

        public ErrorLogsDA(ISqlRepository<ErrorLogs> errorLogs)
        {
            _errorLogs = errorLogs;
        }

        public async Task<IQueryable<ErrorLogs>> GetAll(CancellationToken cancellationToken)
        {
            return await _errorLogs.GetAsync(cancellationToken);
        }

        public async Task<ErrorLogs> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _errorLogs.GetByIdAsync(Id, cancellationToken);
        }
    }
}