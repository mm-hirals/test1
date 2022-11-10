using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("WrkImportCustomers")]
    public class WrkImportCustomers
    {
        [Key]
        public long WrkCustomerID { get; set; }

        public long WrkImportFileID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PrimaryContactNumber { get; set; }
        public string? AlternateContactNumber { get; set; }
        public string? EmailID { get; set; }
        public string? GSTNo { get; set; }
        public string? Street1 { get; set; }
        public string? Stree2 { get; set; }
        public string? Landmark { get; set; }
        public string? Area { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public int Status { get; set; }
        public string ErrorMessage { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}