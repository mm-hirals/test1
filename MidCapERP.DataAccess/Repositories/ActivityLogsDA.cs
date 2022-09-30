using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class ActivityLogsDA : IActivityLogsDA
    {
        private readonly ISqlRepository<ActivityLogs> _ActivityLogs;
        public ActivityLogsDA(ISqlRepository<ActivityLogs> ActivityLogs)
        {
            _ActivityLogs = ActivityLogs;
        }
        public async Task<ActivityLogs> SaveActivityLogs(ActivityLogs activityLogs, CancellationToken cancellationToken)
        {
            return await _ActivityLogs.InsertAsync(activityLogs, cancellationToken);
        }
    }
}
