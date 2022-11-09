using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.User
{
    public class UserRequestDto
    {
        public string Id { get; set; }

        [Required]
        [DisplayName("First Name")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Maximum 50 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [DisplayName("Last Name")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Maximum 50 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [DisplayName("User Name")]
        [StringLength(256, MinimumLength = 1, ErrorMessage = "Maximum 256 characters.")]
        public string UserName { get; set; } = string.Empty;

        [DisplayName("Email Address")]
        [EmailAddress(ErrorMessage = "Please enter valid email address.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "The E-mail is not valid.")]
        [Remote("DuplicateUserEmail", "User", AdditionalFields = nameof(Id), ErrorMessage = ("Email already exist. Please enter a different Email."))]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Please Enter 8 to 16 With Upper Case, Lower Case and Special Character.")]
        public string? Password { get; set; } = string.Empty;

        [DisplayName("Phone Number")]
        [MaxLength(10)]
        [MinLength(10, ErrorMessage = "Please enter 10 digit")]
        [Remote("DuplicateUserPhoneNumber", "User", AdditionalFields = nameof(Id), ErrorMessage = ("Phone Number already exist. Please enter a different Phone Number."))]
        public string PhoneNumber { get; set; } = string.Empty;

        public int UserTenantMappingId { get; set; }

        public int TenantId { get; set; }

        [DisplayName("User Role")]
        public string AspNetRole { get; set; } = string.Empty;

        public string NormalizedName { get; set; } = string.Empty;

        public int? UserId { get; set; }

        [DisplayName("Device UDID")]
        public string? MobileDeviceId { get; set; }
    }
}