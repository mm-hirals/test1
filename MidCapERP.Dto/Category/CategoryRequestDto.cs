using MidCapERP.Dto.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Category
{
    public class CategoryRequestDto
    {
        //[Required(ErrorMessage ="Value is required")]
        //public int LookupValueId { get; set; }
        public int LookupId { get; set; } = (int)MasterPagesEnum.Category;

        [Required(ErrorMessage = "Category Name is required")]
        [DisplayName("Category Name")]
        public string LookupValueName { get; set; }

        //public bool IsDeleted { get; set; }
        //public int CreatedBy { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime CreatedUTCDate { get; set; }
        //public int? UpdatedBy { get; set; }
        //public DateTime? UpdatedDate { get; set; }
        //public DateTime? UpdatedUTCDate { get; set; }
    }
}