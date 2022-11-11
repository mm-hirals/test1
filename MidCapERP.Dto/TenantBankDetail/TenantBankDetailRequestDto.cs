using Microsoft.AspNetCore.Http;
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
        [StringLength(100, MinimumLength = 1)]
        public string BankName { get; set; }

        [Required]
        [DisplayName("Account Name")]
        [StringLength(100, MinimumLength = 1)]
        public string AccountName { get; set; }

        [Required]
        [DisplayName("Account No")]
        [StringLength(50, MinimumLength = 1)]
        public string AccountNo { get; set; }

        [Required]
        [DisplayName("Branch Name")]
        [StringLength(100, MinimumLength = 1)]
        public string BranchName { get; set; }

        [Required]
        [DisplayName("Account Type")]
        [StringLength(50, MinimumLength = 1)]
        public string AccountType { get; set; }

        [Required]
        [DisplayName("IFSC Code")]
        [StringLength(20, MinimumLength = 1)]
        public string IFSCCode { get; set; }

        [DisplayName("UPI Id")]
        [StringLength(200, MinimumLength = 1)]
        public string? UPIId { get; set; }

        public string? QRCode { get; set; }

        [DisplayName("QR Code")]
        public IFormFile? QRImageUpload { get; set; }

        public string IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedUTCDate { get; set; }
        public string UpdatedBy { get; set; }
        public string? UpdatedDate { get; set; }
        public string? UpdatedUTCDate { get; set; }
    }
}