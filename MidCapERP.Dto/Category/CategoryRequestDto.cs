using System.ComponentModel;

namespace MidCapERP.Dto.Category
{
    public class CategoryRequestDto
    {
        public int LookupValueId { get; set; }
        public int LookupId { get; set; }

        [DisplayName("Category Name")]
        public string LookupValueName { get; set; }

        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}