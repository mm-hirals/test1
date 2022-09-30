using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IActivityLogsDA
    {
        public Task<ActivityLogs> SaveActivityLogs(ActivityLogs activityLogs, CancellationToken cancellationToken);
    }
}
