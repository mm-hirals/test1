using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.TenantSMTPDetail
{
    public class TenantSMTPDetailResponseDto
    {
        public long TenantSMTPDetailId { get; set; }
        public long TenantID { get; set; }

        [Required]
        [DisplayName("From Email")]
        public string FromEmail { get; set; }

        [Required]
        [DisplayName("Username")]
        public string Username { get; set; }

        [Required]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required]
        [DisplayName("SMTPServer")]
        public string SMTPServer { get; set; }

        [Required]
        [DisplayName("SMTPPort")]
        public int SMTPPort { get; set; }

        [Required]
        [DisplayName("EnableSSL")]
        public bool EnableSSL { get; set; }

        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}