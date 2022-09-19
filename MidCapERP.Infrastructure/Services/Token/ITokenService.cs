using MidCapERP.DataEntities.Models;
using MidCapERP.Infrastructure.Identity.Models;
using System.Security.Claims;

namespace MidCapERP.Infrastructure.Services.Token
{
    /// <summary>
    ///     A collection of token related services
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        ///     Validate the credentials entered when logging in.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        Task<TokenResponse> Authenticate(TokenRequest request, string ipAddress, CancellationToken cancellationToken, bool isCookie = false);

        /// <summary>
        ///     If the refresh token is valid, a new JWT token will be issued containing the user details.
        /// </summary>
        /// <param name="refreshToken">An existing refresh token.</param>
        /// <param name="ipAddress">The users current ip</param>
        /// <returns>
        ///     <see cref="TokenResponse" />
        /// </returns>
        Task<TokenResponse> RefreshToken(string refreshToken, string ipAddress, CancellationToken cancellationToken);

        /// <summary>
        ///     Check if the credentials passed in are valid.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <param name="password">The matching password to verify.</param>
        /// <returns>If the credentials are valid or not.</returns>
        Task<bool> IsValidUser(string username, string password, CancellationToken cancellationToken);

        /// <summary>
        ///     Find an <see cref="ApplicationUser" /> by their email.
        /// </summary>
        /// <param name="email">
        ///     <see cref="ApplicationUser.Email" />
        /// </param>
        /// <returns>
        ///     <see cref="ApplicationUser" />
        /// </returns>
        Task<ApplicationUser> GetUserByEmail(string email, CancellationToken cancellationToken);

        /// <summary>
        /// Get Token details from claim principles
        /// </summary>
        /// <param name="claimsPrincipal">HttpContext.User</param>
        /// <returns></returns>
        TokenResponse GetTokenDetails(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken);
        Task GenerateOTP(TokenOtpGenerateRequest request, CancellationToken cancellationToken);
        Task<TokenResponse> AuthenticateAPI(TokenAPIRequest request, CancellationToken cancellationToken);
        Task Logout();
    }
}