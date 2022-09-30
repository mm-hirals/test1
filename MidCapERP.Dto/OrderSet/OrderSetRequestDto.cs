using MidCapERP.Dto.OrderSetItem;
using System.Text.Json.Serialization;

namespace MidCapERP.Dto.OrderSet
{
    public class OrderSetRequestDto
    {
       
        public long OrderSetId { get; set; }

        [JsonIgnore]
        public long OrderId { get; set; }

        public string SetName { get; set; }

        [JsonIgnore]
        public long CreatedBy { get; set; }

        [JsonIgnore]
        public DateTime CreatedDate { get; set; }

        [JsonIgnore]
        public DateTime CreatedUTCDate { get; set; }

        [JsonIgnore]
        public int? UpdatedBy { get; set; }

        [JsonIgnore]
        public DateTime? UpdatedDate { get; set; }

        [JsonIgnore]
        public DateTime? UpdatedUTCDate { get; set; }

        public List<OrderSetItemRequestDto> OrderSetItemRequestDto { get; set; }
    }
}