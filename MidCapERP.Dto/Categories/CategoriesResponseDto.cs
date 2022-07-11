using System.ComponentModel;

namespace MidCapERP.Dto.Categories
{
    public class CategoriesResponseDto
    {
        public int CategoryID { get; set; }

        [DisplayName("Category Name")]
        public string CategoryName { get; set; }

        [DisplayName("Active")]
        public bool IsDeleted { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}