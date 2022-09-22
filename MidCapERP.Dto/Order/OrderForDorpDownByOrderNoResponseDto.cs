namespace MidCapERP.Dto.Order
{
    public class OrderForDorpDownByOrderNoResponseDto
    {
        public OrderForDorpDownByOrderNoResponseDto(long id, string orderNo, string type)
        {
            Id = id;
            OrderNo = orderNo;
            Type = type;
        }

        public long Id { get; set; }
        public string OrderNo { get; set; }
        public string Type { get; set; }
    }
}