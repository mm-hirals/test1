using MidCapERP.Dto.DataGrid;
using Newtonsoft.Json;

namespace MidCapERP.Dto.InteriorAddresses
{
    public class InteriorAddressDataTableFilterDto : DataTableFilterDto
    {
        [JsonProperty("customerId")]
        public int customerId { get; set; }
    }
}