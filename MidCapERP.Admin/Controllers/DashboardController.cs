using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MidCapERP.Admin.Models;
using MidCapERP.Dto;
using MidCapERP.Infrastructure.Constants;
using MidCapERP.Infrastructure.Localizer.JsonString;
using NToastNotify;
using System.Diagnostics;

namespace MidCapERP.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly CurrentUser _currentUser;
        private readonly IToastNotification _toastNotification;

        public DashboardController(ILogger<DashboardController> logger, CurrentUser currentUser, IToastNotification toastNotification, IStringLocalizer<BaseController> localizer) : base(localizer)
        {
            _currentUser = currentUser;
            _logger = logger;
            _toastNotification = toastNotification;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Dashboard.View)]
        public IActionResult Index(CancellationToken cancellationToken)
        {
            _toastNotification.AddSuccessToastMessage(_localizer[JsonStringResourcesKeys.LoginSuccessFull]);
            var lang = "hi-IN";
            Response.Headers.AcceptLanguage = new Microsoft.Extensions.Primitives.StringValues(lang);
            
            //gu-IN
            //hi-IN

            return View();
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Dashboard.View)]
        public IActionResult IndexNew(CancellationToken cancellationToken)
        {
            return View();
        }

        public IActionResult Privacy(CancellationToken cancellationToken)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(CancellationToken cancellationToken)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}