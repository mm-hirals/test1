using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MidCapERP.Dto.APIResponse;
using MidCapERP.Infrastructure.Identity.Models;
using MidCapERP.Infrastructure.Localizer.JsonString;
using MidCapERP.Infrastructure.Services.Token;

namespace MidCapERP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IStringLocalizer<AuthController> _localizer;

        public AuthController(IStringLocalizer<AuthController> localizer, IHttpContextAccessor httpContextAccessor, ITokenService tokenService)
        {
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
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
    }
}