using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("CustomerTypes")]
    public class CustomerTypes : BaseEntity
    {
        [Key]
        public int CustomerTypeId { get; set; }

        public string Name { get; set; }
    }
}