using MidCapERP.Dto.DataGrid;
using Newtonsoft.Json;

namespace MidCapERP.Dto.ArchitectAddresses
{
    public class ArchitectAddressDataTableFilterDto : DataTableFilterDto
    {
        [JsonProperty("customerId")]
        public int customerId { get; set; }
    }
}