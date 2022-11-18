using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.OrderSetItem
{
    public class OrderSetItemResponseDto
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

        public string ProductTitle { get; set; }
        public string ModelNo { get; set; }

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
        public DateTime? ReceiveDate { get; set; }
        public decimal? ProvidedMaterial { get; set; }
        public DateTime ReceivedDate { get; set; }
        public decimal ReceivedMaterial { get; set; }
        public string ReceivedFrom { get; set; }
        public long ReceivedBy { get; set; }
        public string ReceivedByName { get; set; }
        public string? RecievedComment { get; set; }
    }
}