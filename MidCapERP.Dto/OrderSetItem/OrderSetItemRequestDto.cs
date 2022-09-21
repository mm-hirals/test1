using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.OrderSetItem
{
    public class OrderSetItemRequestDto
    {
        public long OrderSetItemId { get; set; }

        [Required]
        public long OrderId { get; set; }

        [Required]
        public long OrderSetId { get; set; }

        [Required]
        public int SubjectTypeId { get; set; }

        [Required]
        public long SubjectId { get; set; }

        [Required]
        public string ProductImage { get; set; }

        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? Depth { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public decimal DiscountPrice { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public string Comment { get; set; }

        [Required]
        public int Status { get; set; }

        public bool IsDeleted { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}