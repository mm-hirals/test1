using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Order
{
    public class OrderUpdateReceiveMaterialAPI
    {
        [Required]
        public Int64 OrderId { get; set; }

        [Required]
        public Int64 OrderSetId { get; set; }

        [Required]
        public Int64 OrderSetItemId { get; set; }

        [Required]
        public decimal ReceivedMaterial { get; set; }

        [Required]
        public string ReceivedFrom { get; set; }

        [Required]
        public IFormFile MaterialImage { get; set; }

        public string? Comment { get; set; }
    }
}