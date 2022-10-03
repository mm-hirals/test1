using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IActivityLogsDA
    {
        public Task<IQueryable<ActivityLogs>> GetAll(CancellationToken cancellationToken);
        public Task<ActivityLogs> SaveActivityLogs(ActivityLogs activityLogs, CancellationToken cancellationToken);
    }
}
