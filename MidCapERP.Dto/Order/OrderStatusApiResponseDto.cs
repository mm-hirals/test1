using System.ComponentModel;

namespace MidCapERP.Dto.Order
{
    public class OrderStatusApiResponseDto
    {
        public long OrderId { get; set; }

        public long OrderSetItemId { get; set; }

        [DisplayName("Order No")]
        public string? OrderNo { get; set; }

        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }

        [DisplayName("Total Amount")]
        public decimal TotalAmount { get; set; }

        [DisplayName("order Status")]
        public string OrderStatus { get; set; }

        [DisplayName("Order Date")]
        public DateTime OrderDate { get; set; }
    }
}