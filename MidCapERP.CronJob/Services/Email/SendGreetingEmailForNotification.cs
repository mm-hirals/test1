using MagnusMinds.Utility.EmailService;
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
        private readonly ISubjectTypesDA _subjectTypesDA;
        private readonly ITenantSMTPDetailDA _tenantSMTPDetailDA;
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
            _subjectTypesDA = scope.ServiceProvider.GetRequiredService<ISubjectTypesDA>();
            _tenantSMTPDetailDA = scope.ServiceProvider.GetRequiredService<ITenantSMTPDetailDA>();
        }

        public async Task SendGreetingEmail(CancellationToken cancellationToken)
        {
            List<DataEntities.Models.NotificationManagement> notificationManagementData = _notificationManagementDA.GetAll(cancellationToken).Result.Where(x => x.Status == 0 && x.NotificationMethod == "Email").ToList();
            if (notificationManagementData.Count > 0)
            {
                foreach (var item in notificationManagementData)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(item.ReceiverEmail))
                        {
                            List<string> emailList = new List<string>();
                            emailList.Add(item.ReceiverEmail);

                            var customerId = await _subjectTypesDA.GetById(Convert.ToInt32(item.EntityTypeID), cancellationToken);
                            var tenantSMTPDetailData = await _tenantSMTPDetailDA.GetAll(cancellationToken);
                            var tenantSMTPDetail = tenantSMTPDetailData.FirstOrDefault(x => x.TenantID == customerId.TenantId);

                            if (tenantSMTPDetail != null && !string.IsNullOrEmpty(tenantSMTPDetail.FromEmail) && !string.IsNullOrEmpty(tenantSMTPDetail.SMTPServer) && !string.IsNullOrEmpty(tenantSMTPDetail.Username) && !string.IsNullOrEmpty(tenantSMTPDetail.Password) && tenantSMTPDetail.SMTPPort > 0)
                            {
                                EmailSender emailSender = new EmailSender(new EmailConfiguration()
                                {
                                    From = tenantSMTPDetail.FromEmail,
                                    Password = tenantSMTPDetail.Password,
                                    Port = tenantSMTPDetail.SMTPPort,
                                    SmtpServer = tenantSMTPDetail.SMTPServer,
                                    UserName = tenantSMTPDetail.Username,
                                    UseSSL = tenantSMTPDetail.EnableSSL
                                });

                                EmailHelper _email = new EmailHelper(emailSender);
                                await _email.SendEmail(subject: item.MessageSubject, htmlContent: item.MessageBody, to: emailList);
                            }
                            else
                            {
                                await _emailHelper.SendEmail(subject: item.MessageSubject, htmlContent: item.MessageBody, to: emailList);
                            }

                            item.Status = (int)NotificationManagementEnum.Completed;
                            item.UpdatedDate = DateTime.Now;
                            item.UpdatedUTCDate = DateTime.UtcNow;
                            item.UpdatedBy = _currentUser.UserId;
                            await _notificationManagementDA.UpdateNotification((int)item.NotificationManagementID, item, cancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        item.Status = (int)NotificationManagementEnum.Failed;
                        item.UpdatedDate = DateTime.Now;
                        item.UpdatedUTCDate = DateTime.UtcNow;
                        item.UpdatedBy = _currentUser.UserId;
                        await _notificationManagementDA.UpdateNotification((int)item.NotificationManagementID, item, cancellationToken);
                        throw;
                    }
                }
            }
        }
    }
}