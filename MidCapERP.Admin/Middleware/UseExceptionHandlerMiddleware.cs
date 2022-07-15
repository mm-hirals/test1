using MagnusMinds.Utility.EmailService;
using MimeKit;
using System.Net;
using System.Text.Json;
using UAParser;

namespace MidCapERP.Admin.Middleware
{
    public class UseExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<UseExceptionHandlerMiddleware> _logger;
        private IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public UseExceptionHandlerMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            _logger = context.RequestServices.GetService<ILogger<UseExceptionHandlerMiddleware>>();
            _emailSender = context.RequestServices.GetService<IEmailSender>();
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;

                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                //UAParser
                var userAgent = context.Request.Headers["User-Agent"];
                var uaParser = Parser.GetDefault();
                ClientInfo c = uaParser.Parse(userAgent);

                // Use the information about the exception
                var logEntry = new //OnlineScheduling.Entities.LogEntryEntity()
                {
                    RequestPath = context.Request.Host + context.Request.Path,
                    TimeStamp = DateTime.Now,
                    ActionDescriptor = context.Request?.RouteValues?.Values,
                    IpAddress = Convert.ToString(context.Connection.RemoteIpAddress),
                    Message = error.Message,
                    Exception = Convert.ToString(error),
                    Source = error.Source,
                    StackTrace = error.StackTrace,
                    Type = Convert.ToString(error.GetType()),
                    BrowserName = Convert.ToString(c.UserAgent),
                };

                var result = JsonSerializer.Serialize(new { errorMessage = Newtonsoft.Json.JsonConvert.SerializeObject(logEntry) });
                _logger.LogError(result);

                var sendExceptionEmail = Convert.ToString(_configuration["AppSettings:SendExceptionEmail"]);
                var exceptionEmailToList = Convert.ToString(_configuration["AppSettings:ExceptionEmailToList"]);

                if (sendExceptionEmail == "1" && !string.IsNullOrEmpty(exceptionEmailToList))
                {
                    // Send log in email
                    string htmlTableStart = "<table style=\"width: 100 %; border: 1px solid #e5e5e5;\" border=\"1\" cellspacing=\"0\" cellpadding=\"6\">";
                    string htmlTrStart = "<tr>";
                    string htmlTableEnd = "</table>";
                    string htmlTrEnd = "</tr>";
                    string htmlTdStart = "<td>";
                    string htmlTdEnd = "</td>";
                    string htmlContent = htmlTableStart + htmlTrStart + htmlTdStart + "RequestPath" + htmlTdEnd + htmlTdStart + logEntry.RequestPath + htmlTdEnd + htmlTrEnd
                                         + htmlTrStart + htmlTdStart + "TimeStamp" + htmlTdEnd + htmlTdStart + logEntry.TimeStamp + htmlTdEnd + htmlTrEnd
                                         + htmlTrStart + htmlTdStart + "ActionDescriptor" + htmlTdEnd + htmlTdStart + logEntry.ActionDescriptor + htmlTdEnd + htmlTrEnd
                                         + htmlTrStart + htmlTdStart + "IpAddress" + htmlTdEnd + htmlTdStart + logEntry.IpAddress + htmlTdEnd + htmlTrEnd
                                         + htmlTrStart + htmlTdStart + "Source" + htmlTdEnd + htmlTdStart + logEntry.Source + htmlTdEnd + htmlTrEnd
                                         + htmlTrStart + htmlTdStart + "Type" + htmlTdEnd + htmlTdStart + logEntry.Type + htmlTdEnd + htmlTrEnd
                                         + htmlTrStart + htmlTdStart + "Message" + htmlTdEnd + htmlTdStart + logEntry.Message?.Replace("\r\n", Environment.NewLine) + htmlTdEnd + htmlTrEnd
                                         + htmlTrStart + htmlTdStart + "Exception" + htmlTdEnd + htmlTdStart + logEntry.Exception?.Replace("\r\n", Environment.NewLine) + htmlTdEnd + htmlTrEnd
                                         + htmlTrStart + htmlTdStart + "StackTrace" + htmlTdEnd + htmlTdStart + logEntry.StackTrace?.Replace("\r\n", Environment.NewLine) + htmlTdEnd + htmlTrEnd
                                         + htmlTrStart + htmlTdStart + "BrowserName" + htmlTdEnd + htmlTdStart + logEntry.BrowserName + htmlTdEnd + htmlTrEnd
                                         + htmlTableEnd;

                    if (logEntry.Message != "The client has disconnected")
                    {
                        var message = new MimeMessage();
                        var toEmailList = exceptionEmailToList.Split(',');
                        foreach (var item in toEmailList)
                            message.To.Add(new MailboxAddress("MidCap-ERP Error Email", item));
                        message.Subject = "Error Exception | MidCap-ERP | " + DateTime.Now;
                        message.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlContent };
                        Task.Run(async () => { await _emailSender.SendEmailAsync(message); });
                    }
                }
            }
        }
    }
}