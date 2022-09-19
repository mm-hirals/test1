using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.Infrastructure.Identity.Models;
using MidCapERP.Infrastructure.Services.Token;

namespace MidCapERP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(IHttpContextAccessor httpContextAccessor, ITokenService tokenService)
        {
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<ApiResponse> Post(TokenRequest request, CancellationToken cancellationToken)
        {
            request.Username = "kparmar@magnusminds.net";
            request.Password = "Password@1";

            string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            TokenResponse tokenResponse = await _tokenService.Authenticate(request, ipAddress, cancellationToken, false);
            return new ApiResponse(message: "Login successful", result: tokenResponse, statusCode: 200);
        }

        [HttpPost("GenerateOTPForAPI")]
        public async Task<ApiResponse> GenerateOTPForAPI(TokenOtpGenerateRequest request, CancellationToken cancellationToken)
        {
            await _tokenService.GenerateOTP(request, cancellationToken);
            return new ApiResponse(message: "Generate OTP successfully", result: string.Empty, statusCode: 200);
        }

        [HttpPost("AuthenticateAPI")]
        public async Task<ApiResponse> AuthenticateAPI(TokenAPIRequest request, CancellationToken cancellationToken)
        {
            TokenResponse response = await _tokenService.AuthenticateAPI(request, cancellationToken);

            if (response != null)
            {
                return new ApiResponse(message: "token generated successfully.", result: response, statusCode: 200);
            }
            else
            {
                return new ApiResponse(message: "OTP is Invalid", result: null, statusCode: 404);
            }
        }
    }
}