using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Order
{
    public class OrderUpdateStatusAPI
    {
        [Required]
        public Int64 OrderId { get; set; }

        public string? Comments { get; set; }

        [Required]
        public long BillingAddressID { get; set; }

        [Required]
        public long ShippingAddressID { get; set; }
    }
}