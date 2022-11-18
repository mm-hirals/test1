namespace MidCapERP.Dto.Order
{
    public class OrderProductQuantityDto
    {
        public bool Status { get; set; }
        public List<OrderProductQuantity> OrderProductQuantities { get; set; }
    }

    public class OrderProductQuantity
    {
        public long OrderId { get; set; }
        public long ProductQuantityId { get; set; }
        public long ProductId { get; set; }
        public int ProductQuantity { get; set; }
        public int Quantity { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }
}