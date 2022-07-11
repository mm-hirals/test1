using System.ComponentModel;

namespace MidCapERP.Dto.Categories
{
    public class CategoriesRequestDto
    {
        public int CategoryID { get; set; }

        [DisplayName("Category Name")]
        public string CategoryName { get; set; }

        [DisplayName("Active")]
        public bool IsDeleted { get; set; }
    }
}