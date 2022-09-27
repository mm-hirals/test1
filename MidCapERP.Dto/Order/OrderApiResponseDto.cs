using MidCapERP.Dto.OrderSet;
using System.Text.Json.Serialization;

namespace MidCapERP.Dto.Order
{
    public class OrderApiResponseDto
    {
        [JsonIgnore]
        public long OrderId { get; set; }

        public string OrderNo { get; set; }
        public long CustomerID { get; set; }

        public decimal GrossTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal GSTTaxAmount { get; set; }
        public decimal PayableAmount { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Comments { get; set; }
        public string GSTNo { get; set; }
        public int Status { get; set; }

        public List<OrderSetApiResponseDto> OrderSetApiResponseDto { get; set; }

        [JsonIgnore]
        public int CreatedBy { get; set; }

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
    }
}