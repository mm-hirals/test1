using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;

namespace MidCapERP.DataAccess.Repositories
{
    public class NotificationManagementDA : INotificationManagementDA
    {
        private readonly ISqlRepository<NotificationManagement> _notificationManagement;
        private readonly CurrentUser _currentUser;

        public NotificationManagementDA(ISqlRepository<NotificationManagement> notificationManagement, CurrentUser currentUser)
        {
            _notificationManagement = notificationManagement;
            _currentUser = currentUser;
        }

        public async Task<IQueryable<NotificationManagement>> GetAll(CancellationToken cancellationToken)
        {
            return await _notificationManagement.GetAsync(cancellationToken);
        }

        public async Task<NotificationManagement> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _notificationManagement.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<NotificationManagement> CreateNotification(NotificationManagement model, CancellationToken cancellationToken)
        {
            return await _notificationManagement.InsertAsync(model, cancellationToken);
        }

        public async Task<NotificationManagement> UpdateNotification(int Id, NotificationManagement model, CancellationToken cancellationToken)
        {
            return await _notificationManagement.UpdateAsync(model, cancellationToken);
        }
    }
}