using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Contractors
{
    public class ContractorsRequestDto
    {
        public int ContractorId { get; set; }

        [DisplayName("Contractor Name")]
        public string ContractorName { get; set; }

        [DisplayName("Phone Number")]
        [MaxLength(10)]
        [MinLength(10, ErrorMessage = "Please enter 10 digits")]
        public string PhoneNumber { get; set; }

        [MaxLength(15)]
        public string IMEI { get; set; }

        [DisplayName("Email Address")]
        [EmailAddress(ErrorMessage = "Please enter valid email address")]
        public string EmailId { get; set; }

        public int? TenantId { get; set; }

        [DisplayName("Deleted")]
        public bool IsDeleted { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}