using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MidCapERP.Core.Constants;
using MidCapERP.Core.Localizer.JsonString;
using MidCapERP.Infrastructure.Identity.Models;
using MidCapERP.Infrastructure.Services.Token;
using NToastNotify;

namespace MidCapERP.Admin.Controllers
{
    public class AccountController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IToastNotification _toastNotification;
        private readonly IStringLocalizer<BaseController> _localizer;

        public AccountController(IHttpContextAccessor httpContextAccessor, ITokenService tokenService, IToastNotification toastNotification, IStringLocalizer<BaseController> localizer)
        {
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
            _toastNotification = toastNotification;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Users.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Login(CancellationToken cancellationToken)
        {
            return View();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(TokenRequest request, CancellationToken cancellationToken)
        {
            string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            TokenResponse tokenResponse = await _tokenService.Authenticate(request, ipAddress, cancellationToken, true);

            if (tokenResponse == null)
            {
                _toastNotification.AddErrorToastMessage("Incorrect username or Password. Please try again.");
                return RedirectToAction("Login", "Account");
            }

            _toastNotification.AddSuccessToastMessage(_localizer[JsonStringResourcesKeys.LoginSuccessFull]);
            return RedirectToAction("Index", "Dashboard");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ForgotPassword(CancellationToken cancellationToken)
        {
            return View();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ResetPassword(CancellationToken cancellationToken)
        {
            return View();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> CreateToken(TokenRequest request, CancellationToken cancellationToken)
        {
            request.Username = "kparmar@magnusminds.net";
            request.Password = "Password@1";

            string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            TokenResponse tokenResponse = await _tokenService.Authenticate(request, ipAddress, cancellationToken, false);

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Profile(CancellationToken cancellationToken)
        {
            return View();
        }

        /// <summary>
        /// Logout Method
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            await _tokenService.Logout();
            foreach (var item in Request.Cookies)
            {
                Response.Cookies.Delete(item.Key);
            }
            return RedirectToAction("Index", "Dashboard");
        }
    }
}