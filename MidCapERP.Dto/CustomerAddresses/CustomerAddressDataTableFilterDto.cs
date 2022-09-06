using MidCapERP.Dto.DataGrid;
using Newtonsoft.Json;

namespace MidCapERP.Dto.CustomerAddresses
{
    public class CustomerAddressDataTableFilterDto : DataTableFilterDto
    {
        [JsonProperty("customerId")]
        public int customerId { get; set; }
    }
}