namespace MidCapERP.Dto.TenantSMTPDetail
{
    public class TenantSMTPDetailRequestDto
    {
        public long TenantSMTPDetailId { get; set; }
        public long TenantID { get; set; }
        public string FromEmail { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SMTPServer { get; set; }
        public int SMTPPort { get; set; }
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