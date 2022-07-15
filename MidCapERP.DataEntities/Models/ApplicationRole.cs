using Microsoft.AspNetCore.Identity;

namespace MidCapERP.DataEntities.Models
{
    public class ApplicationRole : IdentityRole
    {
        public int TenantId { get; set; }
        public ApplicationRole()
        {
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
        }
        public ApplicationRole(string roleName, int tenantId) : base(roleName)
        {
            TenantId = tenantId;
        }
    }
}