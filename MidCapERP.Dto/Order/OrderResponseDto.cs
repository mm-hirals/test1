using MidCapERP.Dto.OrderSet;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Order
{
    public class OrderResponseDto
    {
        public long OrderId { get; set; }

        [Required]
        [DisplayName("Order No")]
        public string OrderNo { get; set; }

        [Required]
        public long CustomerID { get; set; }

        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }

        [Required]
        [DisplayName("Gross Total")]
        public decimal GrossTotal { get; set; }

        [Required]
        public decimal Discount { get; set; }

        [Required]
        public decimal ReferralDiscount { get; set; }

        [Required]
        [DisplayName("Order Amount")]
        public decimal TotalAmount { get; set; }

        [Required]
        public decimal GSTTaxAmount { get; set; }

        [Required]
        public decimal PayableAmount { get; set; }

        [Required]
        public DateTime DeliveryDate { get; set; }

        public string? Comments { get; set; }

        public string? GSTNo { get; set; }

        [Required]
        [DisplayName("Order Status")]
        public int Status { get; set; }

        [Required]
        public bool IsDraft { get; set; }

        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }

        [DisplayName("Salesman Name")]
        public string CreatedByName { get; set; }

        public DateTime CreatedDate { get; set; }

        [DisplayName("Order Date")]
        public string CreatedDateFormat => CreatedDate.ToLongDateString();

        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }

        public List<OrderSetResponseDto> OrderSetResponseDto { get; set; }
    }
}