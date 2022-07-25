using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("ErrorLogs")]
    public class ErrorLogs
    {
        [Key]
        public long ErrorLogId { get; set; }
        public int UserId { get; set; }
        public int TenantId { get; set; }
        public string RequestPath { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorException { get; set; }
        public string ErrorStackTrace { get; set; }
        public string ErrorType { get; set; }
        public string BrowserName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
    }
}
