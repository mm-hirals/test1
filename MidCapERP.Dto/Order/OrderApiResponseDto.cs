using MidCapERP.Dto.OrderSet;

namespace MidCapERP.Dto.Order
{
    public class OrderApiResponseDto
    {
        public long OrderId { get; set; }
        public string? OrderNo { get; set; }
        public long CustomerID { get; set; }
        public long BillingAddressID { get; set; }
        public long ShippingAddressID { get; set; }
        public decimal GrossTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal GSTTaxAmount { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal PayableAmount { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string Comments { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<OrderSetApiResponseDto> OrderSetApiResponseDto { get; set; }
    }
}