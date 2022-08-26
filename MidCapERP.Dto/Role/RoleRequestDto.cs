using MidCapERP.Dto.RolePermission;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Role
{
    public class RoleRequestDto
    {
        public RoleRequestDto()
        {
            ModulePermissionList = new List<RolePermissionRequestDto>();
        }

        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
        public int? TenantId { get; set; }

        public List<RolePermissionRequestDto> ModulePermissionList { get; set; }
    }
}