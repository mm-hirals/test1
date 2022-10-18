using System.ComponentModel;

namespace MidCapERP.Dto.ActivityLogs
{
    public class ActivityLogsResponseDto
    {
        public int ActivityLogID { get; set; }
        public int SubjectTypeId { get; set; }
        public string SubjectId { get; set; } = default!;

        [DisplayName("Description")]
        public string Description { get; set; } = default!;

        [DisplayName("Action")]
        public string Action { get; set; } = default!;
        public int CreatedBy { get; set; }

        [DisplayName("Activity Date")]
        public DateTime CreatedDate { get; set; }

        public DateTime CreatedUTCDate { get; set; }
        
        [DisplayName("Activity By")]
        public string CreatedByName { get; set; }
    }
}