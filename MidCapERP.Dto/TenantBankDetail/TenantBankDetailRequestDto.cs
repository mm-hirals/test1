using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.TenantBankDetail
{
	public class TenantBankDetailRequestDto
    {
		public int TenantBankDetailId { get; set; }
		public int TenantId { get; set; }
		[Required]
		[DisplayName("Bank Name")]
		public string BankName { get; set; }
		[Required]
		[DisplayName("Account Name")]
		public string AccountName { get; set; }
		[Required]
		[DisplayName("Account No")]
		public string AccountNo { get; set; }
		[Required]
		[DisplayName("Branch Name")]
		public string BranchName { get; set; }
		[Required]
		[DisplayName("Account Type")]
		public string AccountType { get; set; }
		[Required]
		[DisplayName("IFSC Code")]
		public string IFSCCode { get; set; }
		[DisplayName("UPI Id")]
		public string? UPIId { get; set; }
		[DisplayName("QR Code")]
		public string? QRCode { get; set; }
		public string IsDeleted { get; set; }
		public string CreatedBy { get; set; }
		public string CreatedDate { get; set; }
		public string CreatedUTCDate { get; set; }
		public string UpdatedBy { get; set; }
		public string? UpdatedDate { get; set; }
		public string? UpdatedUTCDate { get; set; }
	}
}
