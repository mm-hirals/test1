using MidCapERP.Dto.DataGrid;
using Newtonsoft.Json;

namespace MidCapERP.Dto.Unit
{
    public class UnitDataTableFilterDto : DataTableFilterDto
    {
        [JsonProperty("unit Name")]
        public string UnitName { get; set; }
    }
}