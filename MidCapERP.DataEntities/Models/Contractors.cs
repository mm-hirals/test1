using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("Contractors")]
    public class Contractors : BaseEntity
    {
        [Key]
        public int ContractorId { get; set; }

        public string ContractorName { get; set; }
        public string PhoneNumber { get; set; }
        public string IMEI { get; set; }
        public string EmailId { get; set; }
        public int? TenantId { get; set; }
    }
}