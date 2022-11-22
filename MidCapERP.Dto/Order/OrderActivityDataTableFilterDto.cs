using MidCapERP.Dto.DataGrid;
using Newtonsoft.Json;

namespace MidCapERP.Dto.Order
{
    public class OrderActivityDataTableFilterDto : DataTableFilterDto
    {
        [JsonProperty("productId")]
        public long orderId { get; set; }
    }
}