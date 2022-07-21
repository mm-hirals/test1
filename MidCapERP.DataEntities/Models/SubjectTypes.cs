using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
