using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using static MidCapERP.Core.Constants.ApplicationIdentityConstants.Permissions;

namespace MidCapERP.DataAccess.Repositories
{
    public class ActivityLogsDA : IActivityLogsDA
    {
        private readonly ISqlRepository<ActivityLogs> _ActivityLogs;
        public ActivityLogsDA(ISqlRepository<ActivityLogs> ActivityLogs)
        {
            _ActivityLogs = ActivityLogs;
        }

        public async Task<IQueryable<ActivityLogs>> GetAll(CancellationToken cancellationToken)
        {
            return await _ActivityLogs.GetAsync(cancellationToken);
        }

        public async Task<ActivityLogs> SaveActivityLogs(ActivityLogs activityLogs, CancellationToken cancellationToken)
        {
            return await _ActivityLogs.InsertAsync(activityLogs, cancellationToken);
        }
    }
}
