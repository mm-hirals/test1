using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface INotificationManagementDA
    {
        public Task<IQueryable<NotificationManagement>> GetAll(CancellationToken cancellationToken);

        public Task<NotificationManagement> GetById(int Id, CancellationToken cancellationToken);

        public Task<NotificationManagement> CreateNotification(NotificationManagement model, CancellationToken cancellationToken);

        public Task<NotificationManagement> UpdateNotification(int Id, NotificationManagement model, CancellationToken cancellationToken);
    }
}