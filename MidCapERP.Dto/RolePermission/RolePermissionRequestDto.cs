using MidCapERP.DataEntities.Models;
using System.ComponentModel;

namespace MidCapERP.Dto.RolePermission
{
    public class RolePermissionRequestDto : ApplicationRole
    {
        [DisplayName("Claim Value")]
        public string claimValue { get; set; } = string.Empty;

        public string AspNetRole { get; set; } = string.Empty;
    }
}