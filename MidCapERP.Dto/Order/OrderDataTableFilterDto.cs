using MidCapERP.Dto.DataGrid;
using Newtonsoft.Json;

namespace MidCapERP.Dto.Order
{
    public class OrderDataTableFilterDto : DataTableFilterDto
    {
        [JsonProperty("refferedBy")]
        public long? RefferedBy { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("status")]
        public int? Status { get; set; }

        [JsonProperty("orderFromDate")]
        public DateTime orderFromDate { get; set; }

        [JsonProperty("orderToDate")]
        public DateTime orderToDate { get; set; }

        [JsonProperty("deliveryFromDate")]
        public DateTime DeliveryFromDate { get; set; }

        [JsonProperty("deliveryToDate")]
        public DateTime DeliveryToDate { get; set; }
    }
}