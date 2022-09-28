namespace MidCapERP.Dto.UserTenantMapping
{
    public class UserTenantMappingResponseDto
    {
        public int UserTenantMappingId { get; set; }
        public int UserId { get; set; }
        public int TenantId { get; set; }
        public string TenantName { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}