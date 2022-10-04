using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidCapERP.Dto.OrderCalculation
{
    public class OrderCalculationApiResponseDto
    {
        public int SubjectTypeId { get; set; }
        public long SubjectId { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? Depth { get; set; }
        public decimal? TotalAmount { get; set; }
        public int Quantity { get; set; }
    }
}
