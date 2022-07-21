using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidCapERP.Dto.Contractors
{
    public class ContractorsRequestDto
    {
        public int ContractorId { get; set; }

        [DisplayName("Contractor Name")]
        public string ContractorName { get; set; }
        public string PhoneNumber { get; set; }
        public string IMEI { get; set; }
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
