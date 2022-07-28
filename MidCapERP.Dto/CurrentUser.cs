namespace MidCapERP.Dto
{
    public class CurrentUser
    {
        public string FullName { get; set; }
        public string RoleId { get; set; }
        public string Role { get; set; }
        public string EmailAddress { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public int TenantId { get; set; }
        public string TenantName { get; set; }
        public string ProfilePic { get; set; }
    }
}