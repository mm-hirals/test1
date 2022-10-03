using MidCapERP.Dto.DataGrid;
using Newtonsoft.Json;

namespace MidCapERP.Dto.Unit
{
    public class UnitDataTableFilterDto : DataTableFilterDto
    {
        [JsonProperty("Unit Name")]
        public string UnitName { get; set; }
    }
}