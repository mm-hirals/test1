using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("ActivityLogs")]
    public class ActivityLogs
    {
        [Key]
        public int ActivityLogID { get; set; }
        public int SubjectTypeId { get; set; }
        public string SubjectId { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Action { get; set; } = default!;
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
    }
}
