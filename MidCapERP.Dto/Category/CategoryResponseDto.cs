using System.ComponentModel;

namespace MidCapERP.Dto.Category
{
    public class CategoryResponseDto
    {
        public long CategoryId { get; set; }
        public long CategoryTypeId { get; set; }
        [DisplayName("Category Name")]
        public string? CategoryName { get; set; }
        public int LookupValueId { get; set; }
        public int LookupId { get; set; }

        [DisplayName("Lookup Name")]
        public string LookupName { get; set; }

        [DisplayName("Category Name")]
        public string LookupValueName { get; set; }
        public bool IsDeleted { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}