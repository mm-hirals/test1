using MidCapERP.Dto.DataGrid;
using Newtonsoft.Json;

namespace MidCapERP.Dto.Fabric
{
    public class FabricDataTableFilterDto : DataTableFilterDto
    {
        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Model")]
        public string Model { get; set; }

        [JsonProperty("Company")]
        public string Company { get; set; }
    }
}