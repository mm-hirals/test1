using System.ComponentModel;

namespace MidCapERP.Dto.Lookups
{
    public class LookupsRequestDto
    {
        public int LookupId { get; set; }

        [DisplayName("Lookup Name")]
        public string LookupName { get; set; }

        public int TenantId { get; set; }

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