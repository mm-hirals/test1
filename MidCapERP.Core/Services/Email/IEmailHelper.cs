namespace MidCapERP.Core.Services.Email
{
    public interface IEmailHelper
    {
        Task SendEmail(string subject, string htmlContent, List<string> to, List<string> cc = null, List<string> bcc = null);
    }
}