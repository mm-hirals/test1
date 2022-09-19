namespace MidCapERP.Dto.Frames
{
    public class OrderResponseDto
    {
        public Int64 OrderId { get; set; }

        public string OrderNo { get; set; }
        public Int64 CustomerID { get; set; }
        public decimal GrossTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal ReferralDiscount { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal GSTTaxAmount { get; set; }
        public decimal PayableAmount { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Comments { get; set; }
        public string GSTNo { get; set; }
        public int Status { get; set; }
        public bool IsDraft { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}