using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
	[Table("TenantBankDetails")]
	public class TenantBankDetail : BaseEntity
	{
		[Key]
		public long TenantBankDetailId { get; set; }
		public long TenantId { get; set; }
		public string BankName { get; set; }
		public string AccountName { get; set; }
		public string AccountNo { get; set; }
		public string BranchName { get; set; }
		public string AccountType { get; set; }
		public string IFSCCode { get; set; }
		public string? UPIId { get; set; }
		public string? QRCode { get; set; }

	}
}
