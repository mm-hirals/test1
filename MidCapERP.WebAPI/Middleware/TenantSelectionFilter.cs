using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MidCapERP.Core.Constants;

namespace MidCapERP.WebAPI.Middleware
{
    public class TenantSelectionFilterAttribute : TypeFilterAttribute
    {
        public TenantSelectionFilterAttribute() : base(typeof(TenantSelectionActionFilter))
        {
        }

        private class TenantSelectionActionFilter : IActionFilter
        {
            private readonly ILogger<TenantSelectionActionFilter> _logger;

            public TenantSelectionActionFilter(ILogger<TenantSelectionActionFilter> logger)
            {
                _logger = logger;
            }

            public async void OnActionExecuting(ActionExecutingContext context)
            {
                if (context.HttpContext.Request.Cookies[ApplicationIdentityConstants.TenantCookieName] == null)
                {
                    RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();
                    redirectTargetDictionary.Add("action", "Index");
                    redirectTargetDictionary.Add("controller", "Tenant");
                    redirectTargetDictionary.Add("area", "");

                    context.Result = new RedirectToRouteResult(redirectTargetDictionary);
                    await context.Result.ExecuteResultAsync(context);
                }
                _logger.LogInformation($"- {nameof(TenantSelectionActionFilter)}.{nameof(OnActionExecuting)}");
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
                _logger.LogInformation($"- {nameof(TenantSelectionActionFilter)}.{nameof(OnActionExecuted)}");
            }
        }
    }
}