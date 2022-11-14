using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.Repositories;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Core.Localizer.JsonString;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.Dto;
using MidCapERP.Dto.APIResponse;
using MidCapERP.Infrastructure.Identity.Models;
using MidCapERP.Infrastructure.Services.Token;
using static MagnusMinds.Utility.ApiDefaultResponseModel;

namespace MidCapERP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IStringLocalizer<AuthController> _localizer;
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly CurrentUser _currentUser;

        public AuthController(IStringLocalizer<AuthController> localizer, IHttpContextAccessor httpContextAccessor, ITokenService tokenService, IUnitOfWorkBL unitOfWorkBL, CurrentUser currentUser)
        {
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
            _unitOfWorkBL = unitOfWorkBL;
            _currentUser = currentUser;
        }

        [HttpPost]
        public async Task<MidCapAPIResponse> Post(TokenRequest request, CancellationToken cancellationToken)
        {
            request.Username = "kparmar@magnusminds.net";
            request.Password = "Password@1";

            string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            TokenResponse tokenResponse = await _tokenService.Authenticate(request, ipAddress, cancellationToken, false);
            return new MidCapAPIResponse(messageCode: JsonStringResourcesKeys.LoginSuccessFull, message: _localizer[JsonStringResourcesKeys.LoginSuccessFull], result: tokenResponse, statusCode: 200);
        }

        [HttpPost("GenerateOTPForAPI")]
        public async Task<MidCapAPIResponse> GenerateOTPForAPI(TokenOtpGenerateRequest request, CancellationToken cancellationToken)
        {
            await _tokenService.GenerateOTP(request, cancellationToken);
            return new MidCapAPIResponse(messageCode: JsonStringResourcesKeys.GenerateOTPSuccessfully, message: _localizer[JsonStringResourcesKeys.GenerateOTPSuccessfully], result: string.Empty, statusCode: 200);
        }

        [HttpPost("AuthenticateAPI")]
        public async Task<MidCapAPIResponse> AuthenticateAPI(TokenAPIRequest request, CancellationToken cancellationToken)
        {
            TokenResponse response = await _tokenService.AuthenticateAPI(request, cancellationToken);

            if (response != null)
            {
                return new MidCapAPIResponse(messageCode: JsonStringResourcesKeys.TokenGeneratedSuccessfully, message: _localizer[JsonStringResourcesKeys.TokenGeneratedSuccessfully], result: response, statusCode: 200);
            }
            else
            {
                return new MidCapAPIResponse(messageCode: JsonStringResourcesKeys.OTPInvalid, message: _localizer[JsonStringResourcesKeys.OTPInvalid], result: null, statusCode: 404);
            }
        }

        [HttpGet("GetPermissions")]
        [Authorize]
        public async Task<ApiResponse> GetPermissionsAPI(CancellationToken cancellationToken)
         {
            var allPermissions = ApplicationIdentityConstants.Permissions.GetAllPermissions();
            var rolePermissionResponseDto = await _unitOfWorkBL.RolePermissionBL.GetRolePermissions(_currentUser.RoleId, allPermissions, cancellationToken);
            var appPortalDetails = rolePermissionResponseDto.Where(a => a.ApplicationType == "App").ToList();
            var permissionsDetails = await _unitOfWorkBL.RolePermissionBL.GetPermissions(appPortalDetails, cancellationToken);
            if (permissionsDetails == null)
            {
                return new ApiResponse(message: "No Data found", result: permissionsDetails, statusCode: 404);
            }
            return new ApiResponse(message: "Data found", result: permissionsDetails, statusCode: 200);
        }
    }
}