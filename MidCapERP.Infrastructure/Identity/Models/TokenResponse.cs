using MidCapERP.DataEntities.Models;

namespace MidCapERP.Infrastructure.Identity.Models
{
    public class TokenResponse
    {
        public TokenResponse()
        { }

        public TokenResponse(ApplicationUser user,
                             string role,
                             string token,
                             string tenantId
                            //string refreshToken
                            )
        {
            this.Id = user.Id;
            this.FullName = user.FullName;
            this.EmailAddress = user.Email;
            this.Token = token;
            this.Role = role;
            this.RoleId = RoleId;
            this.TenantId = tenantId;
            //RefreshToken = refreshToken;
        }

        public string Id { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public string RoleId { get; set; }
        public string TenantId { get; set; }

        //[JsonIgnore]
        //public string RefreshToken { get; set; }
    }
}