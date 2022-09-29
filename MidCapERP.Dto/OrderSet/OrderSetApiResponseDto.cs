using MidCapERP.Dto.OrderSetItem;
using System.Text.Json.Serialization;

namespace MidCapERP.Dto.OrderSet
{
    public class OrderSetApiResponseDto
    {
        [JsonIgnore]
        public long OrderSetId { get; set; }

        [JsonIgnore]
        public long OrderId { get; set; }

        public string SetName { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderSetItemApiResponseDto> OrderSetItemResponseDto { get; set; }
    }
}