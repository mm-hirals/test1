using System.ComponentModel;

namespace MidCapERP.Dto.Contractors
{
    public class ContractorsResponseDto
    {
        public int ContractorId { get; set; }

        [DisplayName("Contractor Name")]
        public string ContractorName { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        public string IMEI { get; set; }

        [DisplayName("Email Address")]
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