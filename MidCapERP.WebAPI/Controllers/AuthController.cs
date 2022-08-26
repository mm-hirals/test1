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

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        public async Task<TokenResponse> Post(TokenRequest request, CancellationToken cancellationToken)
        {
            request.Username = "kparmar@magnusminds.net";
            request.Password = "Password@1";

            string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            TokenResponse tokenResponse = await _tokenService.Authenticate(request, ipAddress, cancellationToken, false);

            return tokenResponse;
        }
    }
}