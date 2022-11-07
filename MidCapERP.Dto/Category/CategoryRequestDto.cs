using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Category
{
    public class CategoryRequestDto
    {
        public int LookupValueId { get; set; }

        //public int LookupId { get; set; } = (int)MasterPagesEnum.Category;

        [Required(ErrorMessage = "The Category Name is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Minimum 1 characters, Maximum 50 characters")]
        [Remote("DuplicateCategoryName", "Category", AdditionalFields = nameof(LookupValueId), ErrorMessage = "Category Name already exist. Please enter a different Category Name.")]
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