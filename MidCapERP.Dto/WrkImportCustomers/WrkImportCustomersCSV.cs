using CsvHelper.Configuration.Attributes;

namespace MidCapERP.Dto.WrkImportCustomers
{
    public class WrkImportCustomersCSV
    {
        [Index(0)]
        public string? FirstName { get; set; }

        [Index(1)]
        public string? LastName { get; set; }

        [Index(2)]
        public string? PrimaryContactNumber { get; set; }

        [Index(3)]
        public string? AlternateContactNumber { get; set; }

        [Index(4)]
        public string? EmailID { get; set; }

        [Index(5)]
        public string? GSTNo { get; set; }

        [Index(6)]
        public string? Street1 { get; set; }

        [Index(7)]
        public string? Stree2 { get; set; }

        [Index(8)]
        public string? Landmark { get; set; }

        [Index(9)]
        public string? Area { get; set; }

        [Index(10)]
        public string? City { get; set; }

        [Index(11)]
        public string? State { get; set; }

        [Index(12)]
        public string? PinCode { get; set; }
    }
}