using MidCapERP.Dto.DataGrid;
using Newtonsoft.Json;

namespace MidCapERP.Dto.Fabric
{
    public class FabricDataTableFilterDto : DataTableFilterDto
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }
    }
}