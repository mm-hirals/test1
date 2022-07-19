using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
	[Table("Lookups")]
    public class Lookups : BaseEntity
	{
        [Key]
		public int LookupId { get; set; }
		public string LookupName { get; set; }
		public int TenantId { get; set; }
	}

}
