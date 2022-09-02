using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.Admin.Models;
using MidCapERP.Dto;
using MidCapERP.Infrastructure.Constants;
using NToastNotify;
using System.Diagnostics;

namespace MidCapERP.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly CurrentUser _currentUser;
        private readonly IToastNotification _toastNotification;

        public DashboardController(ILogger<DashboardController> logger, CurrentUser currentUser, IToastNotification toastNotification)
        {
            _currentUser = currentUser;
            _logger = logger;
            _toastNotification = toastNotification;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Dashboard.View)]
        public IActionResult Index(CancellationToken cancellationToken)
        {
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