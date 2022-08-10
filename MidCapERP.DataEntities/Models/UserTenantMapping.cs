using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("UserTenantMapping")]
    public class UserTenantMapping : BaseEntity
    {
        [Key]
        public int UserTenantMappingId { get; set; }

        public int UserId { get; set; }
        public int TenantId { get; set; }
    }
}