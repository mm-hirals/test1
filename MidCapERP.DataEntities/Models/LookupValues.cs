using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("LookupValues")]
    public class LookupValues : BaseEntity
    {
        [Key]
        public int LookupValueId { get; set; }

        public int LookupId { get; set; }
        public string LookupValueName { get; set; }
    }
}