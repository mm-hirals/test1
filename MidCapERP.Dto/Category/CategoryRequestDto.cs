using Microsoft.AspNetCore.Mvc;
using MidCapERP.Dto.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Category
{
    public class CategoryRequestDto
    {
        public long CategoryId { get; set; }

        [Required(ErrorMessage = "The Category Type is required")]
        [DisplayName("Category Type")]
        public int CategoryTypeId { get; set; } = (int)ProductCategoryTypesEnum.Product;

        [Required(ErrorMessage = "The Category Name is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Minimum 1 characters, Maximum 50 characters")]
        [Remote("DuplicateCategoryName", "Category", AdditionalFields = nameof(CategoryId), ErrorMessage = "Category Name already exist. Please enter a different Category Name.")]
        [DisplayName("Category Name")]
        public string? CategoryName { get; set; }

        [DisplayName("Fixed Price")]
        public bool IsFixedPrice { get; set; }

        [Required(ErrorMessage = "The RSP Percentage is required")]
        [DisplayName("RSP Percentage")]
        [StringLength(6)]
        public string? RSPPercentage { get; set; } = "0";

        [Required(ErrorMessage = "The WSP Percentage is required")]
        [DisplayName("WSP Percentage")]
        [StringLength(6)]
        public string? WSPPercentage { get; set; } = "0";

        public int TenantId { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}