using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidCapERP.Dto.Order
{
    public class OrderDeleteApiRequestDto
    {
        public long OrderId { get; set; }
        public long OrderSetId { get; set; }
        public long OrderSetItemId { get; set; }
        public int DeleteType { get; set; }
    }
}
