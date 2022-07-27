using System.ComponentModel;

namespace MidCapERP.Dto.Status
{
    public class StatusRequestDto
    {
        public int StatusId { get; set; }

        [DisplayName("StatusTitle")]
        public string StatusTitle { get; set; }

        public string StatusDescription { get; set; }
        public int StatusOrder { get; set; }
        public int TenantId { get; set; }

        [DisplayName("IsCompleted")]
        public bool IsCompleted { get; set; }

        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}