using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.Admin.Models;
using MidCapERP.Infrastructure.Constants;
using System.Diagnostics;

namespace MidCapERP.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Dashboard.View)]
        public IActionResult Index(CancellationToken cancellationToken)
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