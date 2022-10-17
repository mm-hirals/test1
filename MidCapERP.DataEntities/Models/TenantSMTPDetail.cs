using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("TenantSMTPDetails")]
    public class TenantSMTPDetail : BaseEntity
    {
        [Key]
        public long TenantSMTPDetailId { get; set; }

        public long TenantID { get; set; }
        public string FromEmail { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SMTPServer { get; set; }
        public int SMTPPort { get; set; }
        public bool EnableSSL { get; set; }
    }
}