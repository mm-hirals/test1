using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("AccessoriesTypes")]
    public class AccessoriesTypes : BaseEntity
    {
        [Key]
        public int AccessoriesTypeId { get; set; }
        public int CategoryId { get; set; }
        public string TypeName { get; set; }
        public int TenantId { get; set; }
    }
}
