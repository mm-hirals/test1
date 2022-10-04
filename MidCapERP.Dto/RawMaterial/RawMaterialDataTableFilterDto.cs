using MidCapERP.Dto.DataGrid;
using Newtonsoft.Json;

namespace MidCapERP.Dto.RawMaterial
{
    public class RawMaterialDataTableFilterDto : DataTableFilterDto
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("unit Name")]
        public string UnitName { get; set; }
    }
}