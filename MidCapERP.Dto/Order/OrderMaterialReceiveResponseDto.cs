namespace MidCapERP.Dto.Order
{
    public class OrderMaterialReceiveResponseDto
    {
        public long OrderId { get; set; }
        public string OrderNo { get; set; }
        public string CustomerName { get; set; }
        public string MobileNo { get; set; }
        public long OrderSetItemId { get; set; }
        public string OrderSetName { get; set; }
        public string OrderSetComment { get; set; }
        public string ReceivedFrom { get; set; }
        public decimal ProvidedMaterial { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string ReceivedBy { get; set; }
        public string? ReceivedComment { get; set; }
        public List<OrderSetItemImageResponseDto> OrderSetItemImageResponseDto { get; set; }
    }

    public class OrderSetItemImageResponseDto
    {
        public long OrderSetItemImageId { get; set; }

        public long OrderSetItemId { get; set; }
        public string? DrawImage { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
    }
}