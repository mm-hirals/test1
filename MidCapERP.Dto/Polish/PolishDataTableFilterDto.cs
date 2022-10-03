using MidCapERP.Dto.DataGrid;
using Newtonsoft.Json;

namespace MidCapERP.Dto.Polish
{
    public class PolishDataTableFilterDto : DataTableFilterDto
    {
        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Model")]
        public string Model { get; set; }

        [JsonProperty("Company")]
        public string Company { get; set; }
    }
}