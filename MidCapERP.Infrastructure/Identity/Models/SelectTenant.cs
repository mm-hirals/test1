using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Infrastructure.Identity.Models
{
    public class SelectTenant
    {
        [Required]
        [DisplayName("Tenant")]
        public string TenantId { get; set; } = string.Empty;
    }
}