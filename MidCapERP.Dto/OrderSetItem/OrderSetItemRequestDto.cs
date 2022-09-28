using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MidCapERP.Dto.OrderSetItem
{
    public class OrderSetItemRequestDto
    {
        
        public long OrderSetItemId { get; set; }

        [JsonIgnore]
        [Required]
        public long OrderId { get; set; }

        [JsonIgnore]
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

        [JsonIgnore]
        public bool IsDeleted { get; set; }

        [JsonIgnore]
        public long CreatedBy { get; set; }

        [JsonIgnore]
        public DateTime CreatedDate { get; set; }

        [JsonIgnore]
        public DateTime CreatedUTCDate { get; set; }

        [JsonIgnore]
        public long? UpdatedBy { get; set; }

        [JsonIgnore]
        public DateTime? UpdatedDate { get; set; }

        [JsonIgnore]
        public DateTime? UpdatedUTCDate { get; set; }
    }
}