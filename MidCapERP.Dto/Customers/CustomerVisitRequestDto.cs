using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MidCapERP.Dto.Customers
{
    public class CustomerVisitRequestDto
    {
        public long CustomerId { get; set; }
        public string? Comment { get; set; }
    }
}
