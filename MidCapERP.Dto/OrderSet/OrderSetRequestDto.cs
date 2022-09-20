namespace MidCapERP.Dto.OrderSet
{
    public class OrderSetRequestDto
    {
        public long OrderSetId { get; set; }
        public long OrderId { get; set; }
        public string SetName { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsDeleted { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}