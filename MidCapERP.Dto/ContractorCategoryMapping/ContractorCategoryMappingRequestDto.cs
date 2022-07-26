using System.ComponentModel;

namespace MidCapERP.Dto.ContractorCategoryMapping
{
    public class ContractorCategoryMappingRequestDto
	{
		public int ContractorCategoryMappingId { get; set; }
		public int ContractorId { get; set; }
		public int CategoryId { get; set; }

		[DisplayName("Deleted")]
		public bool IsDeleted { get; set; }
	}
}