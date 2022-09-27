using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;

namespace MidCapERP.DataAccess.Repositories
{
    public class ErrorLogsDA : IErrorLogsDA
    {
        private readonly ISqlRepository<ErrorLogs> _errorLogs;
        private readonly CurrentUser _currentUser;

        public ErrorLogsDA(ISqlRepository<ErrorLogs> errorLogs, CurrentUser currentUser)
        {
            _errorLogs = errorLogs;
            _currentUser = currentUser;
        }

        public async Task<IQueryable<ErrorLogs>> GetAll(CancellationToken cancellationToken)
        {
            return await _errorLogs.GetAsync(cancellationToken, x => x.TenantId == _currentUser.TenantId);
        }

        public async Task<ErrorLogs> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _errorLogs.GetByIdAsync(Id, cancellationToken);
        }
    }
}