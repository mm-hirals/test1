namespace MidCapERP.Dto.TenantBankDetail
{
    public class TenantBankDetailResponseDto
    {
		public long TenantBankDetailID { get; set; }
		public long TenantID { get; set; }
		public string BankName { get; set; }
		public string AccountName { get; set; }
		public string AccountNo { get; set; }
		public string BranchName { get; set; }
		public string AccountType { get; set; }
		public string IFSCCode { get; set; }
		public string? UPIId { get; set; }
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
