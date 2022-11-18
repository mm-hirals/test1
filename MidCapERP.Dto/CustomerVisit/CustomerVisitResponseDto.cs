namespace MidCapERP.Dto.CustomerVisit
{
    public class CustomerVisitResponseDto
    {
        public long CustomerId { get; set; }
        public string? Comment { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}