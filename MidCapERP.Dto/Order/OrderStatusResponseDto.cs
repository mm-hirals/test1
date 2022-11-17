namespace MidCapERP.Dto.Order
{
    public class OrderStatusResponseDto
    {
        public long OrderId { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}