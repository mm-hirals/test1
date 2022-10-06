using MidCapERP.Dto.DataGrid;
using Newtonsoft.Json;

namespace MidCapERP.Dto.Role
{
    public class RoleDataTableFilterDto : DataTableFilterDto
    {
        [JsonProperty("roleName")]
        public string RoleName { get; set; }
    }
}