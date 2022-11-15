using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Repositories;
using MidCapERP.Core.Constants;
using MidCapERP.Core.Localizer.JsonString;
using MidCapERP.Dto.User;
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
        private readonly IUserBL _userBL;
        private readonly IConfiguration _configuration;

        public AccountController(IHttpContextAccessor httpContextAccessor, ITokenService tokenService, IToastNotification toastNotification, IStringLocalizer<BaseController> localizer,IUserBL userBL, IConfiguration configuration)
        {
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
            _toastNotification = toastNotification;
            _userBL = userBL;
            _configuration = configuration;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.PortalUser.View)]
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
        public async Task<IActionResult> Login(string returnUrl, CancellationToken cancellationToken)
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
        public async Task<IActionResult> Login(TokenRequest request, string returnUrl, CancellationToken cancellationToken)
        {
            string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            TokenResponse tokenResponse = await _tokenService.Authenticate(request, ipAddress, cancellationToken, true);

            if (tokenResponse == null)
            {
                _toastNotification.AddErrorToastMessage("Incorrect username or Password. Please try again.");
                return RedirectToAction("Login", "Account");
            }
            if (string.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Index", "Dashboard");
            else
                return Redirect(returnUrl);
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
        /// Forgot Password functionality
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string userName, CancellationToken cancellationToken)
        {
            if (userName != null || userName != string.Empty)
            {
                UserResponseDto  user = await _userBL.GetUserByUsername(userName,cancellationToken);
                
                if(user != null)
                {
                    
                    string forgotPasswordUrl = _configuration["AppSettings:HostURL"] + MagnusMinds.Utility.Encryption.Encrypt(Convert.ToString(user.UserId), true, ApplicationIdentityConstants.EncryptionSecret);
                    var renderedString = await RazorTemplateEngine.RenderAsync("~/Views/Account/ForgotPasswordEmailTemplate.cshtml", forgotPasswordUrl);
                    List<string> emailList = new List<string>();
                    emailList.Add("mm.rutulp@gmail.com");

                    await _userBL.SendForgotPasswordMail(emailList,renderedString,cancellationToken);
                    return RedirectToAction("Login", "Account");
                }
                throw new Exception("User not found");
            }
            else
            {
                throw new Exception("User not found");
            }
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