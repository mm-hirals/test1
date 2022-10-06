using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;

namespace MidCapERP.BusinessLogic.Services.ActivityLog
{
    public class ActivityLogsService : IActivityLogsService
    {
        private readonly IUnitOfWorkDA _unitOfWorkDA;
        private readonly CurrentUser _currentUser;

        public ActivityLogsService(IUnitOfWorkDA unitOfWorkDA, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _currentUser = currentUser;
        }

        public async Task<ActivityLogs> PerformActivityLog(int subjectTypeId, long subjectId, string description, string action, CancellationToken cancellationToken)
        {
            var dataActivityLog = MapActivityLogs(subjectTypeId, subjectId, description, action);
            var activityLog = await _unitOfWorkDA.ActivityLogsDA.SaveActivityLogs(dataActivityLog, cancellationToken);
            return activityLog;
        }

        private ActivityLogs MapActivityLogs(int subjectTypeId, long subjectId, string description, string action)
        {
            return new ActivityLogs() { SubjectTypeId = subjectTypeId, SubjectId = subjectId, Description = $"{description} - By {_currentUser.FullName} on {DateTime.Now}", Action = action, CreatedBy = _currentUser.UserId, CreatedDate = DateTime.Now, CreatedUTCDate = DateTime.UtcNow };
        }
    }
}