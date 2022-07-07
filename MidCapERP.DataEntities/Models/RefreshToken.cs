using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MidCapERP.DataEntities.Models
{
    [Owned]
    public class RefreshToken
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        public string Token { get; set; }
        public DateTime Expiry { get; set; }

        public bool IsExpired
        {
            get { return DateTime.UtcNow >= Expiry; }
        }

        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime? Revoked { get; set; }
        public string RevokedByIp { get; set; }
        public string ReplacedByToken { get; set; }

        public bool IsActive
        {
            get { return Revoked == null && !IsExpired; }
        }
    }
}