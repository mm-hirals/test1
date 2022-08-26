namespace MidCapERP.Dto.Role
{
    public class RoleResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
        public int? TenantId { get; set; }
    }
}