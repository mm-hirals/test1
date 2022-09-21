namespace MidCapERP.Dto.Order
{
    public class OrderForDorpDownByOrderNoResponseDto
    {
        public OrderForDorpDownByOrderNoResponseDto(long id, string type, string orderNo)
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