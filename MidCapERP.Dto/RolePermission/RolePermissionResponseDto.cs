using MidCapERP.DataEntities.Models;

namespace MidCapERP.Dto.RolePermission
{
    public class RolePermissionResponseDto 
    {
        public string Module { get; set; }
        public List<PermissiongResponseDto> ModulePermissionList { get; set; }
    }

    public class PermissiongResponseDto : ApplicationRole
    {
        public string Permission { get; set; }
        public string PermissionType { get; set; }
        public string IsChecked { get; set; }
    }
}
