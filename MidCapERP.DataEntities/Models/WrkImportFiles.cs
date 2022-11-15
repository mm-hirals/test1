using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("WrkImportFiles")]
    public class WrkImportFiles
    {
        [Key]
        public long WrkImportFileID { get; set; }

        public int TenantId { get; set; }
        public string FileType { get; set; }
        public string ImportFileName { get; set; }
        public int TotalRecords { get; set; }
        public int? Success { get; set; }
        public int? Failed { get; set; }
        public DateTime? ProcessStartDate { get; set; }
        public DateTime? ProcessEndDate { get; set; }
        public int Status { get; set; }
        public string? ErrorMessage { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}