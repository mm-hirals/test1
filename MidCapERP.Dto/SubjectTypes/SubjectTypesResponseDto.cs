using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidCapERP.Dto.SubjectTypes
{
    public  class SubjectTypesResponseDto
    {
		public int SubjectTypeId { get; set; }

		[DisplayName("SubjectType Name")]
		public string SubjectTypeName { get; set; }

		[DisplayName("Comment")]
		public string Comments { get; set; }
		public int TenantId { get; set; }

		[DisplayName("IsDeleted")]
		public bool IsDeleted { get; set; }
		public int CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime CreatedUTCDate { get; set; }
		public int? UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public DateTime? UpdatedUTCDate { get; set; }
	}
}
