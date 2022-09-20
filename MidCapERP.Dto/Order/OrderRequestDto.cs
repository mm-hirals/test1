using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Order
{
    public class OrderRequestDto
    {
        public long OrderId { get; set; }

        [Required]
        public string OrderNo { get; set; }

        [Required]
        public Int64 CustomerID { get; set; }

        [Required]
        public decimal GrossTotal { get; set; }

        [Required]
        public decimal Discount { get; set; }

        [Required]
        public decimal ReferralDiscount { get; set; }

        [Required]
        public decimal? TotalAmount { get; set; }

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

        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}