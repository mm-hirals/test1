using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
