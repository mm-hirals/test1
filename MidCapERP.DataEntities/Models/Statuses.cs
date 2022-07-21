using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("Statuses")]
    public class  Statuses :BaseEntity
    {

        [Key]
        public int StatusId { get; set; }
        public string StatusTitle { get; set; }
        public string StatusDescription { get; set; }
        public int StatusOrder { get; set; }
        public bool IsCompleted { get; set; }
        public int TenantId { get; set; }

    }
}
