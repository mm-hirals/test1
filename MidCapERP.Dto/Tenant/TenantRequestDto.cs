using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidCapERP.Dto.Tenant
{
    public class TenantRequestDto
    {
        
        public int TenantId { get; set; }
        [DisplayName("Tenant Name")]
        public string TenantName { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
       
        public string EmailId { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        
        public string? LogoPath { get; set; }
        [DisplayName("Street Address1")]
        public string StreetAddress1 { get; set; }
        [DisplayName("Street Address2")]
        public string? StreetAddress2 { get; set; }
        
        public string? Landmark { get; set; }
    
        public string City { get; set; }
      
        public string State { get; set; }
 
        public string Pincode { get; set; }
        public string IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedUTCDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public string UpdatedUTCDate { get; set; }
        public string WebsiteURL { get; set; }
        public string TwitterURL { get; set; }
        public string FacebookURL { get; set; }
        public string InstagramURL { get; set; }
        [DisplayName("GST No")]
        public string GSTNo { get; set; }
 
        public string RetailerPercentage { get; set; }
        public string WholeSellerPercentage { get; set; }
        public string ArchitectDiscount { get; set; }
    }
}
