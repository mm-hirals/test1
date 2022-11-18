using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("OrderAnonymousViews")]

    public class OrderAnonymousView
    {
        [Key]
        public long OrderAnonymousViewId { get; set; }
        public long OrderId { get; set; }
        public long CustomerId { get; set; }
        public string IPAddress { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
    }
}
