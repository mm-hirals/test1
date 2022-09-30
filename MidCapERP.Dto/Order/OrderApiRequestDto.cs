using MidCapERP.Dto.OrderSet;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MidCapERP.Dto.Order
{
    public class OrderApiRequestDto
    {
        public long OrderId { get; set; }

        [JsonIgnore]
        public string? OrderNo { get; set; }

        [Required]
        public long CustomerID { get; set; }

        [Required]
        public decimal GrossTotal { get; set; }

        [Required]
        public decimal Discount { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public decimal GSTTaxAmount { get; set; }

        [JsonIgnore]
        public DateTime? DeliveryDate { get; set; }

        public string Comments { get; set; }

        [JsonIgnore]
        public int Status { get; set; }

        [JsonIgnore]
        [Required]
        public int TenantId { get; set; }

        public List<OrderSetRequestDto> OrderSetRequestDto { get; set; }

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