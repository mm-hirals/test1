using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MidCapERP.DataEntities.Models
{
    [Table("SubjectTypes")]
	public class SubjectTypes : BaseEntity
	{
		[Key]
		public int SubjectTypeId { get; set; }
		public string SubjectTypeName { get; set; }
		public string Comments { get; set; }
		public int TenantId { get; set; }
	}
}
