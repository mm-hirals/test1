using MidCapERP.Dto.DataGrid;
using Newtonsoft.Json;

namespace MidCapERP.Dto.Order
{
    public class OrderDataTableFilterDto : DataTableFilterDto
    {
        [JsonProperty("reffereBy")]
        public long? RefferedBy { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("orderDate")]
        public DateTime orderDate { get; set; }

        [JsonProperty("deliveryDate")]
        public DateTime DeliveryDate { get; set; }
    }
}