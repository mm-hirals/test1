using MidCapERP.DataEntities.Models;

namespace MidCapERP.Dto.RolePermission
{
    public class RolePermissionResponseDto
    {
        public string ApplicationType { get; set; }
        public List<PermissiongResponseDto> RolePermissionList { get; set; }
    }

    public class PermissiongResponseDto : ApplicationRole
    {
        public string Module { get; set; }
        public string Permission { get; set; }
        public string PermissionType { get; set; }
        public string IsChecked { get; set; }
    }
}
