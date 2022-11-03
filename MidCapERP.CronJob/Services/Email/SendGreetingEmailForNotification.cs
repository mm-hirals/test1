using Microsoft.Extensions.DependencyInjection;
using MidCapERP.Core.Constants;
using MidCapERP.Core.Services.Email;
using MidCapERP.DataAccess.Interface;
using MidCapERP.Dto;

namespace MidCapERP.CronJob.Services.Email
{
    public class SendGreetingEmailForNotification : ISendGreetingEmailForNotification
    {
        private readonly INotificationManagementDA _notificationManagementDA;

        private readonly IEmailHelper _emailHelper;
        private readonly CurrentUser _currentUser;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SendGreetingEmailForNotification(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            IServiceScope scope = _serviceScopeFactory.CreateScope();
            _emailHelper = scope.ServiceProvider.GetRequiredService<IEmailHelper>();
            _currentUser = scope.ServiceProvider.GetRequiredService<CurrentUser>();
            _notificationManagementDA = scope.ServiceProvider.GetRequiredService<INotificationManagementDA>();
        }

        public async Task SendGreetingEmail(CancellationToken cancellationToken)
        {
            try
            {
                var data = _notificationManagementDA.GetAll(cancellationToken).Result.Where(x => x.Status == 0);
                if (data.Count() > 0)
                {
                    foreach (var item in data.ToList())
                    {
                        if (!string.IsNullOrEmpty(item.ReceiverEmail))
                        {
                            List<string> emailList = new List<string>();
                            emailList.Add(item.ReceiverEmail);
                            await _emailHelper.SendEmail(item.MessageSubject, item.MessageBody, emailList);

                            item.Status = (int)NotificationManagementEnum.Completed;
                            item.UpdatedDate = DateTime.UtcNow;
                            item.UpdatedBy = _currentUser.UserId;
                            await _notificationManagementDA.UpdateNotification((int)item.NotificationManagementID, item, cancellationToken);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}