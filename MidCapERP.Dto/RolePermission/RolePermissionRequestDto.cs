using MidCapERP.DataEntities.Models;

namespace MidCapERP.Dto.RolePermission
{
    public class RolePermissionRequestDto : ApplicationRole
    {
        public string RoleId { get; set; }
        public string Permission { get; set; }
        public string Module { get; set; }
        public string PermissionType { get; set; }
        public string IsChecked { get; set; }

        public List<RolePermissionRequestDto> ModulePermissionList { get; set; }
    }
}