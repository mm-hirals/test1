using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Order
{
    public class OrderUpdateApproveOrDeclineAPI
    {
        [Required]
        public Int64 OrderId { get; set; }
        public bool IsOrderApproved { get; set; }
        public string? Comments { get; set; }
    }
}
