namespace MidCapERP.Dto.ActivityLogs
{
	public class ActivityLogsResponseDto
	{
        public int ActivityLogID { get; set; }
        public int SubjectTypeId { get; set; }
        public string SubjectId { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Action { get; set; } = default!;
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public string CreatedByName { get; set; }
    }
}
