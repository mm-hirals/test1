using System.ComponentModel;

namespace MidCapERP.Dto.Statuses
{
    public class StatusesResponseDto
    {
        public int StatusId { get; set; }

        [DisplayName("StatusTitle")]
        public string StatusTitle { get; set; }

        public string StatusDescription { get; set; }

        public string StatusOrder { get; set; }

        [DisplayName("IsCompleted")]
        public bool IsCompleted { get; set; }

        public int TenantId { get; set; }

        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}