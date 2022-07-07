using MagnusMinds.Utility.EmailService;
using System.Net;
using System.Text.Json;

namespace MidCapERP.Admin.Middleware
{
    public class UseExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<UseExceptionHandlerMiddleware> _logger;
        private IEmailSender _emailSender;

        public UseExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
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

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                _logger.LogError(result);
                //var message = new MimeMessage();
                //message.To.Add(new MailboxAddress("Error Email", "kparmar@magnusminds.net"));
                //message.Body = new TextPart("Plain") { Text = result };
                //try
                //{
                //   await _emailSender.SendEmailAsync(message);
                //}
                //catch (Exception ex)
                //{
                //    throw ex;
                //}
                await response.WriteAsync(result);
            }
        }
    }
}