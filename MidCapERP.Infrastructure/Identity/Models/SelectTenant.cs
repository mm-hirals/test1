using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Infrastructure.Identity.Models
{
    public class SelectTenant
    {
        [Required]
        public string TenantId { get; set; } = string.Empty;
    }
}
