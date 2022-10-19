using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidCapERP.Dto.Order
{
    public class OrderUpdateStatusAPI
    {
        public Int64 OrderId { get; set; }
        public string? Comments { get; set; }
    }
}
