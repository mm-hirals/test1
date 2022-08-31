using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.User
{
    public class UserResponseDto
    {
        public string Id { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; } = string.Empty;

        [DisplayName("Last Name")]
        public string LastName { get; set; } = string.Empty;

        [DisplayName("User Name")]
        public string UserName { get; set; } = string.Empty;

        [DisplayName("Email Address")]
        [EmailAddress(ErrorMessage = "Please enter valid email address")]
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        public int? UserId { get; set; }
    }
}