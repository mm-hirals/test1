namespace MidCapERP.Dto.NotificationManagement
{
    public class NotificationManagementResponseDto
    {
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