using MidCapERP.Dto.DataGrid;
using Newtonsoft.Json;

namespace MidCapERP.Dto.RawMaterial
{
    public class RawMaterialDataTableFilterDto : DataTableFilterDto
    {
        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Unit Name")]
        public string UnitName { get; set; }

    }
}