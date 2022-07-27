using Microsoft.AspNetCore.Identity;
using System.Runtime.Serialization;

namespace MidCapERP.DataEntities.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [IgnoreDataMember]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        //[JsonIgnore]
        //public List<RefreshToken> RefreshTokens { get; set; }
    }
}