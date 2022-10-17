using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MidCapERP.Core.Constants;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Infrastructure.Identity.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MidCapERP.Infrastructure.Services.Token
{
    /// <inheritdoc cref="ITokenService" />
    public class TokenService : ITokenService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly TokenConfiguration _token;
        private readonly HttpContext _httpContext;
        private readonly IOTPLoginDA _loginDA;
        private readonly IUnitOfWorkDA _unitOfWorkDA;

        /// <inheritdoc cref="ITokenService" />
        public TokenService(
            IUnitOfWorkDA unitOfWorkDA,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IOptions<TokenConfiguration> tokenOptions,
            IHttpContextAccessor httpContextAccessor, IOTPLoginDA otpLoginDA)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _token = tokenOptions.Value;
            _loginDA = otpLoginDA;
            _httpContext = httpContextAccessor.HttpContext;
            _unitOfWorkDA = unitOfWorkDA;
        }

        /// <inheritdoc cref="ITokenService.Authenticate(TokenRequest, string)"/>
        public async Task<TokenResponse> Authenticate(TokenRequest request, string ipAddress, CancellationToken cancellationToken, bool isCookie = false)
        {
            if (await IsValidUser(request.Username, request.Password, cancellationToken))
            {
                ApplicationUser user = await GetUser(request.Username, cancellationToken);

                if (user != null)//&& user.IsEnabled)
                {
                    return await GenerateAuthentication(isCookie, user, cancellationToken);
                }
            }

            return null;
        }

        public async Task GenerateOTP(TokenOtpGenerateRequest request, CancellationToken cancellationToken)
        {
            string data = string.Empty;
            var user = await GetUserByPhoneNo(request, cancellationToken);
            if (user != null)
            {
                if (user.MobileDeviceId == request.MobileDeviceId)
                {
                    var otpLogin = await _loginDA.GetAll(cancellationToken);
                    var oldLoginTokenByPhoneNo = otpLogin.FirstOrDefault(p => p.PhoneNumber == request.PhoneNo);

                    if (oldLoginTokenByPhoneNo == null)
                    {
                        OTPLogin loginToken = new OTPLogin()
                        {
                            PhoneNumber = request.PhoneNo,
                            OTP = "0000",// new Random().Next(1, 9999).ToString("D4"),
                            ExpiryTime = DateTime.UtcNow.AddMinutes(10),
                        };
                        var createdToken = await _loginDA.CreateLoginToken(loginToken, cancellationToken);
                        data = createdToken.OTP;
                    }
                    else
                    {
                        oldLoginTokenByPhoneNo.OTP = "0000";
                        //oldLoginTokenByPhoneNo.OTP = new Random().Next(1, 9999).ToString("D4");
                        oldLoginTokenByPhoneNo.ExpiryTime = DateTime.UtcNow.AddMinutes(10);
                        var createdToken = await _loginDA.UpdateLoginToken(oldLoginTokenByPhoneNo, cancellationToken);
                        data = createdToken.OTP;
                    }

                    //Send OTP to Someone through SMS or Email
                }
                else
                {
                    throw new Exception("User device not found");
                }
            }
            else
            {
                throw new Exception("User not found");
            }
        }

        public async Task<TokenResponse> AuthenticateAPI(TokenAPIRequest request, CancellationToken cancellationToken)
        {
            var loginTokens = await _loginDA.GetAll(cancellationToken);
            var oldLoginTokenByPhoneNo = loginTokens.FirstOrDefault(p => p.PhoneNumber == request.PhoneNo);

            if (oldLoginTokenByPhoneNo != null)
            {
                if (oldLoginTokenByPhoneNo.OTP == request.OTP)
                {
                    if (DateTime.UtcNow < oldLoginTokenByPhoneNo.ExpiryTime)
                    {
                        TokenOtpGenerateRequest tokenOtpGenerate = new TokenOtpGenerateRequest();
                        tokenOtpGenerate.PhoneNo = request.PhoneNo;
                        tokenOtpGenerate.MobileDeviceId = request.MobileDeviceId;
                        var user = await GetUserByPhoneNo(tokenOtpGenerate, cancellationToken);
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return await GenerateAuthentication(false, user, cancellationToken);
                    }
                    throw new Exception("OTP was Expired");
                }
                throw new Exception("OTP is wrong");
            }
            throw new Exception("Phone No is not Registered");
        }

        public async Task<ApplicationUser?> GetUserByPhoneNo(TokenOtpGenerateRequest request, CancellationToken cancellationToken)
        {
            var getAllUser = await _unitOfWorkDA.UserDA.GetUsers(cancellationToken);
            var getNullMobileDevice = getAllUser.FirstOrDefault(p => p.PhoneNumber == request.PhoneNo);
            if (getNullMobileDevice == null)
                throw new Exception("User Not Found");
            //if (getNullMobileDevice.MobileDeviceId == null || getNullMobileDevice.MobileDeviceId != request.MobileDeviceId)
            //{
            //    getNullMobileDevice.MobileDeviceId = request.MobileDeviceId;
            //    await _unitOfWorkDA.UserDA.UpdateUser(getNullMobileDevice);
            //}
            return _userManager.Users.FirstOrDefault(p => p.PhoneNumber == request.PhoneNo && p.IsActive && !p.IsDeleted);
        }

        public Task<TokenResponse> RefreshToken(string refreshToken, string ipAddress, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ITokenService.IsValidUser(string, string)" />
        public async Task<bool> IsValidUser(string username, string password, CancellationToken cancellationToken)
        {
            ApplicationUser user = await GetUser(username, cancellationToken);

            if (user == null)
            {
                // Username or password was incorrect.
                return false;
            }

            SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, password, true, false);

            return signInResult.Succeeded;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
            //await _httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        /// <inheritdoc cref="ITokenService.GetUserByEmail(string)" />
        public async Task<ApplicationUser> GetUserByEmail(string email, CancellationToken cancellationToken)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        /// <inheritdoc cref="ITokenService.GetUserByEmail(string)" />
        public async Task<ApplicationUser> GetUserByName(string username, CancellationToken cancellationToken)
        {
            return await _userManager.FindByNameAsync(username);
        }

        /// <inheritdoc cref="ITokenService.GetUser(string)" />
        public async Task<ApplicationUser> GetUser(string searchText, CancellationToken cancellationToken)
        {
            var user = await GetUserByEmail(searchText, cancellationToken);
            if (user == null)
            {
                user = await GetUserByName(searchText, cancellationToken);
            }
            return user;
        }

        public TokenResponse GetTokenDetails(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            var UserId = GetTokenClaim(claimsPrincipal, TokenEnum.UserId.ToString(), cancellationToken)?.Value;
            var FullName = GetTokenClaim(claimsPrincipal, TokenEnum.FullName.ToString(), cancellationToken)?.Value;
            var Name = GetTokenClaim(claimsPrincipal, TokenEnum.Name.ToString(), cancellationToken)?.Value;
            var NameIdentifier = GetTokenClaim(claimsPrincipal, TokenEnum.NameIdentifier.ToString(), cancellationToken)?.Value;
            var Role = GetTokenClaim(claimsPrincipal, TokenEnum.Role.ToString(), cancellationToken)?.Value;
            var RoleId = GetTokenClaim(claimsPrincipal, TokenEnum.RoleId.ToString(), cancellationToken)?.Value;
            var EmailAddress = GetTokenClaim(claimsPrincipal, TokenEnum.EmailAddress.ToString(), cancellationToken)?.Value;

            return new TokenResponse()
            {
                EmailAddress = EmailAddress,
                Id = UserId,
                FullName = FullName,
                Role = Role,
                RoleId = RoleId,
            };
        }

        public Claim GetTokenClaim(ClaimsPrincipal claimsPrincipal, string type, CancellationToken cancellationToken)
        {
            return claimsPrincipal.Claims.FirstOrDefault(c => c.Type == type);
        }

        /// <summary>
        ///     Issue JWT token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<string> GenerateJwtToken(ApplicationUser user, CancellationToken cancellationToken, bool isCookie = false)
        {
            string roleName = (await _userManager.GetRolesAsync(user))[0];
            var roleDetails = await _roleManager.FindByNameAsync(roleName);
            byte[] secret = Encoding.ASCII.GetBytes(_token.Secret);
            var claimsIdentity = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Email),
                    new Claim(ClaimTypes.Role, roleName),

                    new Claim(TokenEnum.EmailAddress.ToString(), user.Email),
                    new Claim(TokenEnum.UserId.ToString(), user.Id),
                    new Claim(TokenEnum.FullName.ToString(), $"{user.FirstName} {user.LastName}"),
                    new Claim(TokenEnum.Name.ToString(), user.Email),
                    new Claim(TokenEnum.NameIdentifier.ToString(), user.Email),
                    new Claim(TokenEnum.Role.ToString(), roleName),
                    new Claim(TokenEnum.RoleId.ToString(), roleDetails.Id)
                }, isCookie ? CookieAuthenticationDefaults.AuthenticationScheme : JwtBearerDefaults.AuthenticationScheme);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Issuer = _token.Issuer,
                Audience = _token.Audience,
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddMinutes(_token.Expiry),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            };

            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            if (isCookie)
            {
                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(_token.Expiry),
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A
                    // value set here overrides the ExpireTimeSpan option of
                    // CookieAuthenticationOptions set with AddCookie.

                    //IsPersistent = true,
                    // Whether the authentication session is persisted across
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http
                    // redirect response value.
                };

                await _httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            }
            SecurityToken token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }

        private async Task<TokenResponse> GenerateAuthentication(bool isCookie, ApplicationUser user, CancellationToken cancellationToken)
        {
            string role = (await _userManager.GetRolesAsync(user))[0];
            string jwtToken = await GenerateJwtToken(user, cancellationToken, isCookie);

            var userTenants = await _unitOfWorkDA.UserTenantMappingDA.GetAll(cancellationToken);
            var tenant = userTenants.FirstOrDefault(p => p.UserId == user.UserId);
            string tenantId = string.Empty;
            if (tenant != null) tenantId = MagnusMinds.Utility.Encryption.Encrypt(Convert.ToString(tenant.TenantId), true, ApplicationIdentityConstants.EncryptionSecret);

            return new TokenResponse(user,
                                     role,
                                     jwtToken,
                                     tenantId
                                     //""//refreshToken.Token
                                     );
        }
    }
}