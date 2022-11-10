namespace MidCapERP.Dto.WrkImportFiles
{
    public class WrkImportFilesDto
    {
        public long WrkImportFileID { get; set; }
        public string FileType { get; set; }
        public string ImportFileName { get; set; }
        public int TotalRecords { get; set; }
        public int? Success { get; set; }
        public int? Failed { get; set; }
        public DateTime? ProcessStartDate { get; set; }
        public DateTime? ProcessEndDate { get; set; }
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