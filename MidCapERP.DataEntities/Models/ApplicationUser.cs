using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace MidCapERP.DataEntities.Models
{
    public class ApplicationUser : IdentityUser
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int UserId { get; set; }

        [IgnoreDataMember]
        [DisplayName("User Name")]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public string? MobileDeviceId { get; set; }

        //[JsonIgnore]
        //public List<RefreshToken> RefreshTokens { get; set; }
    }
}