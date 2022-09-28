using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("Tenants")]
    public class Tenant : BaseEntity
    {
        [Key]
        public int TenantId { get; set; }

        public string TenantName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string PhoneNumber { get; set; }
        public string? LogoPath { get; set; }
        public string StreetAddress1 { get; set; }
        public string? StreetAddress2 { get; set; }
        public string? Landmark { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
    }
}