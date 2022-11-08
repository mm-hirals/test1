using MidCapERP.Dto.DataGrid;
using Newtonsoft.Json;

namespace MidCapERP.Dto.Interior
{
    public class InteriorDataTableFilterDto : DataTableFilterDto
    {
        [JsonProperty("customerName")]
        public string customerName { get; set; }

        [JsonProperty("customerMobileNo")]
        public string customerMobileNo { get; set; }

        [JsonProperty("customerFromDate")]
        public DateTime customerFromDate { get; set; }

        [JsonProperty("customerToDate")]
        public DateTime customerToDate { get; set; }
    }
}