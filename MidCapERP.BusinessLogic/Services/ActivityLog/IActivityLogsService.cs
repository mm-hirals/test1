using MidCapERP.DataEntities.Models;

namespace MidCapERP.BusinessLogic.Services.ActivityLog
{
    public interface IActivityLogsService
    {
        public Task<ActivityLogs> PerformActivityLog(int subjectTypeId, long subjectId, string description, string action, CancellationToken cancellationToken);
    }
}