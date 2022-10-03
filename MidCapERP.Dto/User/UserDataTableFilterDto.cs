using MidCapERP.Dto.DataGrid;
using Newtonsoft.Json;

namespace MidCapERP.Dto.User
{
    public class UserDataTableFilterDto : DataTableFilterDto
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("PhoneNumber")]
        public string PhoneNumber { get; set; }
    }
}