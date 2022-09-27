using MidCapERP.Dto.OrderSet;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MidCapERP.Dto.Order
{
    public class OrderApiRequestDto
    {
        [JsonIgnore]
        public long OrderId { get; set; }

        public string OrderNo { get; set; }

        [Required]
        public long CustomerID { get; set; }

        [Required]
        public decimal GrossTotal { get; set; }

        [Required]
        public decimal Discount { get; set; }

        [Required]
        public decimal ReferralDiscount { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public decimal GSTTaxAmount { get; set; }

        [Required]
        public decimal PayableAmount { get; set; }

        [Required]
        public DateTime DeliveryDate { get; set; }

        public string Comments { get; set; }

        [Required]
        public string GSTNo { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        public bool IsDraft { get; set; }

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