using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("NotificationManagement")]
    public class NotificationManagement
    {
        [Key]
        public long NotificationManagementID { get; set; }

        public long EntityTypeID { get; set; }
        public long EntityID { get; set; }
        public string NotificationType { get; set; }
        public string NotificationMethod { get; set; }
        public string? MessageSubject { get; set; }
        public string? MessageBody { get; set; }
        public string? ReceiverEmail { get; set; }
        public string? ReceiverMobile { get; set; }
        public int Status { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}